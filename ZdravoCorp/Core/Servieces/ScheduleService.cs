using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.Core.Servieces
{
    public class ScheduleService
    {
        private const int TIME_SLOT_TOLERANCE = 1;
        private const int APPOINTMENT_DURATION = 15;

        private IScheduleRepository scheduleRepository;
        private IDoctorRepository doctorRepository;

        public ScheduleService()
        {
            scheduleRepository = Singleton.Instance.ScheduleRepository;
            doctorRepository = Singleton.Instance.DoctorRepository;
        }
        
        public List<Appointment> GetAppointmentsByRequest(AppointmentRequest appointmentRequest, int patientId)
        {
            List<TimeSlot> closestTimeSlots = GetClosestTimeSlots(appointmentRequest, patientId);
            if (closestTimeSlots != null) return GetAppointmentsFromTimeSlot(patientId, appointmentRequest.Doctor, closestTimeSlots[0]);
            if (appointmentRequest.Priority == Priority.Doctor)
            {
                closestTimeSlots = GetClosestTimeSlotsByPriorityDoctor(appointmentRequest, patientId);
                if (closestTimeSlots != null) return GetAppointmentsFromTimeSlot(patientId, appointmentRequest.Doctor, closestTimeSlots[0]);
            }
            else
            {
                List<Appointment> recommendedAppointments = GetClosestAppointmentsByTimeInterval(appointmentRequest, patientId);
                if (recommendedAppointments != null) return recommendedAppointments;
            }
            return GetClosestAppointments(patientId);
        }

        public List<Appointment> GetClosestAppointments(int patientId)
        {
            Patient patient = Singleton.Instance.PatientRepository.getById(patientId);
            List<Appointment> closestAppointments = new List<Appointment>();
            for (DateTime i = DateTime.Now.AddMinutes(15); ; i = i.AddMinutes(1))
            {
                if (closestAppointments.Count() == 3) break;
                GetAppointmentsForDuration(patientId, i, closestAppointments);
            }
            return closestAppointments;
        }

        private void GetAppointmentsForDuration(int patientId, DateTime dateTime, List<Appointment> closestAppointments)
        {
            DoctorService doctorService = new DoctorService();
            PatientService patientService = new PatientService();
            TimeSlot freeTimeSlot = new TimeSlot(dateTime, APPOINTMENT_DURATION);
            foreach (Doctor doctor in doctorRepository.GetDoctors())
            {
                if (!doctorService.IsAvailable(freeTimeSlot, doctor.Id) ||
                    !patientService.IsAvailable(freeTimeSlot, patientId)) continue;
                if (!AppointmentTimeOverlaps(closestAppointments, freeTimeSlot, doctor.Id)) continue;
                Appointment appointment =
                    new Appointment(scheduleRepository.getLastId() + 1, freeTimeSlot, doctor.Id, patientId, "");
                closestAppointments.Add(appointment);
                if (closestAppointments.Count() == 3) break;
            }
        }

        //funkcija koja proverava da li se TimeSlot preklapa sa nekim od TimeSlotova u listi Appointmenta
        public bool AppointmentTimeOverlaps(List<Appointment> appointments, TimeSlot timeSlot, int doctorId)
        {
            foreach (Appointment appointment in appointments)
            {
                if (appointment.TimeSlot.OverlapWith(timeSlot)) return false;
            }
            return true;
        }
 
        public List<Appointment> GetClosestAppointmentsByTimeInterval(AppointmentRequest appointmentRequest, int patientId)
        {
            for (DateTime currentDate = DateTime.Now; currentDate.Date <= appointmentRequest.LatestDate.Date; currentDate = currentDate.AddDays(1))
            {
                foreach (Doctor doctor in doctorRepository.GetDoctors())
                {
                    if (doctor.Id == appointmentRequest.Doctor.Id) continue;
                    DateTime startTime = GetStartTime(currentDate, appointmentRequest.EarliesTime);
                    DateTime endTime = GetEndTime(currentDate, appointmentRequest.LatestTime);
                    int duration = (endTime - startTime).Minutes;
                    List<TimeSlot> timeSlots = new List<TimeSlot>() { new TimeSlot(startTime, duration) };
                    if (scheduleRepository.ContainsKey(currentDate.Date)) GetFreeDoctorsTimeSlots(scheduleRepository.GetAppointmentsByDate(currentDate.Date), timeSlots, doctor.Id);
                    TimeSlot? timeSlot = GetFirstFreeTimeSlot(timeSlots, patientId);
                    if (timeSlot != null) return GetAppointmentsFromTimeSlot(patientId, doctor, timeSlot);
                }
            }
            return null;
        }

        private List<Appointment> GetAppointmentsFromTimeSlot(int patientId, Doctor doctor, TimeSlot timeSlot)
        {
            List<TimeSlot> tempTimeSlot = new List<TimeSlot>() { timeSlot };
            List<Appointment> closestAppointments = new List<Appointment>();
            foreach (var slot in tempTimeSlot)
            {
                closestAppointments.Add(new Appointment(scheduleRepository.getLastId() + 1, slot, doctor.Id, patientId, ""));
            }
            return closestAppointments;
        }

        public List<TimeSlot> GetClosestTimeSlotsByPriorityDoctor(AppointmentRequest appointmentRequest, int patientId)
        {
            appointmentRequest.LatestDate = appointmentRequest.LatestDate.AddDays(TIME_SLOT_TOLERANCE);
            appointmentRequest.EarliesTime = new TimeOnly(0, 0);
            appointmentRequest.LatestTime = new TimeOnly(23, 59);
            return GetClosestTimeSlots(appointmentRequest, patientId);
        }

        public List<TimeSlot> GetClosestTimeSlots(AppointmentRequest appointmentRequest, int patientId)
        {
            for (DateTime currentDate = DateTime.Now; currentDate.Date <= appointmentRequest.LatestDate; currentDate = currentDate.AddDays(1))
            {
                DateTime startTime = GetStartTime(currentDate, appointmentRequest.EarliesTime);
                DateTime endTime = GetEndTime(currentDate, appointmentRequest.LatestTime);
                int duration = (int)(endTime - startTime).TotalMinutes;
                if (duration <= 0) continue;
                List<TimeSlot> timeSlots = new List<TimeSlot>() { new TimeSlot(startTime, duration) };
                if (scheduleRepository.ContainsKey(currentDate.Date)) GetFreeDoctorsTimeSlots(scheduleRepository.GetAppointmentsByDate(currentDate.Date), timeSlots, appointmentRequest.Doctor.Id);
                TimeSlot? timeSlot = GetFirstFreeTimeSlot(timeSlots, patientId);
                if (timeSlot != null) return new List<TimeSlot> { timeSlot };
            }
            return null;
        }
 
        private DateTime GetStartTime(DateTime currentDate, TimeOnly earliestTime)
        {
            DateTime startTime = new DateTime();
            if (currentDate.Date == DateTime.Now.Date && DateTime.Now.TimeOfDay >= earliestTime.ToTimeSpan())
            {
                startTime = currentDate.AddMinutes(15);
            }
            else
            {
                startTime = currentDate.Date.Add(earliestTime.ToTimeSpan());
            }
            return startTime;
        }

        private DateTime GetEndTime(DateTime currentDate, TimeOnly latestTime)
        {
            return currentDate.Date.Add(latestTime.ToTimeSpan());
        }

        public TimeSlot? GetFirstFreeTimeSlot(List<TimeSlot> freeTimeSlots, int patientId)
        {
            for (int i = 0; i < freeTimeSlots.Count; i++)
            {
                if (freeTimeSlots[i].duration < APPOINTMENT_DURATION) continue;
                var founded = MakeTimeSlotForPatient(freeTimeSlots[i], patientId);
                if (founded == null) continue;
                var tempTimeSlots = freeTimeSlots[i].Split(founded);
                UpdateTimeSlots(freeTimeSlots, i, tempTimeSlots);
                return founded;
            }
            return null;
        }

        private static void UpdateTimeSlots(List<TimeSlot> freeTimeSlots, int i, List<TimeSlot> tempTimeSlots)
        {
            freeTimeSlots.Remove(freeTimeSlots[i]);
            for (int j = 0; j < tempTimeSlots.Count; j++)
            {
                freeTimeSlots.Insert(i + j, tempTimeSlots[j]);
            }
        }

        public TimeSlot MakeTimeSlotForPatient(TimeSlot timeSlot, int patientId)
        {
            Patient patient = Singleton.Instance.PatientRepository.getById(patientId);
            for (DateTime currentDate = timeSlot.start; currentDate <= timeSlot.start.AddMinutes(timeSlot.duration - 15); currentDate = currentDate.AddMinutes(1))
            {
                TimeSlot founded = new TimeSlot(currentDate, APPOINTMENT_DURATION);
                PatientService patientService = new PatientService();
                if (!patientService.IsAvailable(founded, patient.Id)) continue;
                return founded;
            }
            return null;
        }

        public void GetFreeDoctorsTimeSlots(List<Appointment> appointments, List<TimeSlot> timeSlots, int doctorId)
        {
            //foreach (Appointment appointment in appointments)
            //    if (doctorId != appointment.DoctorId || appointment.IsCanceled) continue;
            //    SplitTimeSlot(appointment, timeSlots
            foreach (Appointment appointment in scheduleRepository.GetAppointmentsForDoctor(doctorId))
            {
                if (appointment.IsCanceled) continue;
                SplitTimeSlot(appointment, timeSlots);
            }
        }

        public void SplitTimeSlot(Appointment appointment, List<TimeSlot> timeSlots)
        {
            for (int i = 0; i < timeSlots.Count; i++)
            {
                if (timeSlots[i].OverlapWith(appointment.TimeSlot))
                {
                    List<TimeSlot> tempTimeSlots = timeSlots[i].Split(appointment.TimeSlot);
                    timeSlots.Remove(timeSlots[i]);
                    for (int j = 0; j < tempTimeSlots.Count; j++)
                    {
                        timeSlots.Insert(i + j, tempTimeSlots[j]);
                    }
                }
            }
        }

        public List<Appointment> GetAllAppointmentsForDoctor(DateTime startDate, DateTime endDate, int doctorId)
        {
            List<Appointment> doctorsAppointments = new List<Appointment>();

            while (startDate <= endDate)
            {
                foreach (Appointment appointment in scheduleRepository.GetAppointmentsForDoctor(doctorId))
                {
                    if (appointment.TimeSlot.start.Date == startDate)
                    {
                        doctorsAppointments.Add(appointment);
                    }
                }
                startDate = startDate.AddDays(1);
            }
            return doctorsAppointments;
        }







        public Appointment UpdateAppointment(int appointmentId, TimeSlot timeSlot, int doctorId, Patient patient = null)
        {
            return scheduleRepository.UpdateAppointment(appointmentId, timeSlot, doctorId, patient);
        }

        public Appointment CreateAppointment(TimeSlot timeSlot, Doctor doctor, Patient patient, int idExamination = 0)
        {
            string roomId = TakeRoom(timeSlot);
            if (roomId == "")
            {
                MessageBox.Show("All rooms are full.");
                return null;
            }
            return scheduleRepository.CreateAppointment(timeSlot, doctor, patient, roomId, idExamination);
        }


        public string TakeRoom(TimeSlot timeSlot)
        {
            Dictionary<string, Room> examinationRooms = Room.LoadAllExaminationRoom();
            foreach (var room in examinationRooms)
            {
                bool check = true;
                foreach (Appointment appointment in scheduleRepository.GetAppointments())
                {
                    if (appointment.IsCanceled) continue;
                    if (appointment.TimeSlot.OverlapWith(timeSlot) && appointment.IdRoom == room.Key)
                    {
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    return room.Key;
                }
            }
            return "";
        }

        public Appointment GetAppointmentByExaminationId(int examinationId) 
        {
            foreach (Appointment appointment in scheduleRepository.GetAppointments()) 
            {
                if (appointment.ExaminationId == examinationId)
                {
                    return appointment;
                }
            }
            return null;
        }

        public Appointment CreateAppointment(Appointment appointment)
        {
            return scheduleRepository.CreateAppointment(appointment);
        }

        public void WriteAllAppointmens()
        {
            scheduleRepository.WriteAllAppointmens();
        }

        public Appointment GetAppointmentById(int id)
        {
            return scheduleRepository.GetAppointmentById(id);
        }

        public List<Appointment> GetAppointmentsForPatient(int patientId)
        {
            return scheduleRepository.GetAppointmentsForPatient(patientId);
        }

        public Appointment CancelAppointment(int appointmentId)
        {
            return scheduleRepository.CancelAppointment(appointmentId);
        }

        public List<Appointment> GetTodaysAppointments()
        {
            return scheduleRepository.GetTodaysAppointments();
        }
    }
}
