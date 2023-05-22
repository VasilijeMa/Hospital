using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.Enums;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Servieces
{
    public class ScheduleService
    {
        private const int TIME_SLOT_TOLERANCE = 1;
        private const int APPOINTMENT_DURATION = 15;

        ScheduleRepository scheduleRepository = Singleton.Instance.ScheduleRepository;
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
            foreach (Doctor doctor in Singleton.Instance.DoctorRepository.Doctors)
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
                TimeSlotService timeSlotService = new TimeSlotService(appointment.TimeSlot);
                if (timeSlotService.OverlapWith(timeSlot)) return false;
            }
            return true;
        }
        public List<Appointment> GetClosestAppointmentsByTimeInterval(AppointmentRequest appointmentRequest, int patientId)
        {
            for (DateTime currentDate = DateTime.Now; currentDate.Date <= appointmentRequest.LatestDate.Date; currentDate = currentDate.AddDays(1))
            {
                foreach (Doctor doctor in Singleton.Instance.DoctorRepository.Doctors)
                {
                    if (doctor.Id == appointmentRequest.Doctor.Id) continue;
                    DateTime startTime = GetStartTime(currentDate, appointmentRequest.EarliesTime);
                    DateTime endTime = GetEndTime(currentDate, appointmentRequest.LatestTime);
                    int duration = (endTime - startTime).Minutes;
                    List<TimeSlot> timeSlots = new List<TimeSlot>() { new TimeSlot(startTime, duration) };
                    if (scheduleRepository.Schedule.DailyAppointments.ContainsKey(currentDate.Date)) GetFreeDoctorsTimeSlots(scheduleRepository.Schedule.DailyAppointments[currentDate.Date], timeSlots, doctor.Id);
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
                if (scheduleRepository.Schedule.DailyAppointments.ContainsKey(currentDate.Date)) GetFreeDoctorsTimeSlots(scheduleRepository.Schedule.DailyAppointments[currentDate.Date], timeSlots, appointmentRequest.Doctor.Id);
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
                var timeSlotService = new TimeSlotService(freeTimeSlots[i]);
                var tempTimeSlots = timeSlotService.Split(founded);
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
            foreach (Appointment appointment in appointments)
            {
                if (doctorId != appointment.DoctorId || appointment.IsCanceled) continue;
                SplitTimeSlot(appointment, timeSlots);
            }
        }
        public void SplitTimeSlot(Appointment appointment, List<TimeSlot> timeSlots)
        {
            for (int i = 0; i < timeSlots.Count; i++)
            {
                TimeSlotService timeSlotService = new TimeSlotService(timeSlots[i]);
                if (timeSlotService.OverlapWith(appointment.TimeSlot))
                {
                    List<TimeSlot> tempTimeSlots = timeSlotService.Split(appointment.TimeSlot);
                    timeSlots.Remove(timeSlots[i]);
                    for (int j = 0; j < tempTimeSlots.Count; j++)
                    {
                        timeSlots.Insert(i + j, tempTimeSlots[j]);
                    }
                }
            }
        }
    }
}
