using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Ink;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    public class Schedule
    {
        private const int TIME_SLOT_TOLERANCE = 1;
        private const int APPOINTMENT_DURATION = 15;
        public List<Appointment> todaysAppointments { get; set; }
        public List<Appointment> appointments { get; set; }
        public Dictionary<DateTime, List<Appointment>> dailyAppointments { get; set; }
        public Schedule()
        {
            this.appointments = LoadAllAppointments();
            CreateAppointmentsMap();
            this.todaysAppointments = GetTodaysAppontments();
        }

        public Schedule(List<Appointment> appointments)
        {
            this.appointments = appointments;
        }

        public Appointment CreateAppointment(TimeSlot timeSlot, Doctor doctor, Patient patient)
        {
            string roomId = Appointment.TakeRoom(timeSlot);
            if (roomId == "")
            {
                MessageBox.Show("All rooms are full.");
                return null;
            }
            int id = getLastId() + 1;
            Appointment appointment = new Appointment(id, timeSlot, doctor.Id, patient.Id, roomId);
            appointments.Add(appointment);
            if (appointment.TimeSlot.start.Date == DateTime.Now.Date) this.todaysAppointments.Add(appointment);
            CreateAppointmentsMap();
            return appointment;
        }

        public Appointment CreateAppointment(Appointment appointment)
        {
            appointments.Add(appointment);
            CreateAppointmentsMap();
            return appointment;
        }

        public Appointment UpdateAppointment(int appointmentId, TimeSlot timeSlot, int doctorId, Patient patient = null)
        {
            foreach (var appointment in appointments)
            {
                if (appointment.Id == appointmentId)
                {
                    appointment.TimeSlot = timeSlot;
                    appointment.DoctorId = doctorId;
                    if (patient != null)
                    {
                        appointment.PatientId = patient.Id;
                    }
                    return appointment;
                }
            }
            return null;
        }

        public Appointment CancelAppointment(int appointmentId)
        {
            foreach (var appointment in appointments)
            {
                if (appointment.Id == appointmentId)
                {
                    appointment.IsCanceled = true;
                    return appointment;
                }
            }
            return null;
        }

        public List<Appointment> LoadAllAppointments()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/appointments.json");
            var json = reader.ReadToEnd();
            List<Appointment> appointments = JsonConvert.DeserializeObject<List<Appointment>>(json);
            return appointments;
        }

        public void WriteAllAppointmens()
        {
            string json = JsonConvert.SerializeObject(appointments, Formatting.Indented);
            File.WriteAllText("./../../../data/appointments.json", json);
        }

        public int getLastId()
        {
            try
            {
                return appointments.Max(appointment => appointment.Id);
            }
            catch
            {
                return 0;
            }
        }

        public Appointment GetAppointment(int id)
        {
            foreach (var appointment in appointments)
            {
                if (appointment.Id == id)
                {
                    return appointment;
                }
            }
            return null;
        }

        public void CreateAppointmentsMap()
        {
            dailyAppointments = new Dictionary<DateTime, List<Appointment>>();
            foreach (var appointment in appointments)
            {
                if (dailyAppointments.ContainsKey(appointment.TimeSlot.start.Date))
                {
                    dailyAppointments[appointment.TimeSlot.start.Date].Add(appointment);
                }
                else
                {
                    dailyAppointments.Add(appointment.TimeSlot.start.Date, new List<Appointment>());
                    dailyAppointments[appointment.TimeSlot.start.Date].Add(appointment);
                }
                if (dailyAppointments.ContainsKey(appointment.TimeSlot.start.AddMinutes(appointment.TimeSlot.duration).Date))
                {
                    dailyAppointments[appointment.TimeSlot.start.AddMinutes(appointment.TimeSlot.duration).Date].Add(appointment);
                }
                else
                {
                    dailyAppointments.Add(appointment.TimeSlot.start.AddMinutes(appointment.TimeSlot.duration).Date, new List<Appointment>());
                    dailyAppointments[appointment.TimeSlot.start.AddMinutes(appointment.TimeSlot.duration).Date].Add(appointment);
                }
            }
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
            Patient patient = Patient.getById(patientId);
            List<Appointment> closestAppointments = new List<Appointment>();
            for (DateTime i = DateTime.Now.AddMinutes(15); ; i = i.AddMinutes(1))
            {
                if (closestAppointments.Count() == 3) break;
                foreach (Doctor doctor in Singleton.Instance.doctors)
                {
                    TimeSlot freeTimeSlot = new TimeSlot(i, APPOINTMENT_DURATION);
                    if (doctor.IsAvailable(freeTimeSlot) && patient.IsAvailable(freeTimeSlot))
                    {
                        if (!AppointmentTimeOverlaps(closestAppointments, freeTimeSlot, doctor.Id)) continue;
                        Appointment appointment = new Appointment(getLastId() + 1, freeTimeSlot, doctor.Id, patientId, "");
                        closestAppointments.Add(appointment);
                        if (closestAppointments.Count() == 3) break;
                    }
                }
            }
            return closestAppointments;
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
                foreach (Doctor doctor in Singleton.Instance.doctors)
                {
                    if (doctor.Id == appointmentRequest.Doctor.Id) continue;
                    DateTime startTime = GetStartTime(currentDate, appointmentRequest.EarliesTime);
                    DateTime endTime = GetEndTime(currentDate, appointmentRequest.LatestTime);
                    int duration = (int)(endTime - startTime).Minutes;
                    List<TimeSlot> timeSlots = new List<TimeSlot>() { new TimeSlot(startTime, duration) };
                    if (dailyAppointments.ContainsKey(currentDate.Date)) GetFreeDoctorsTimeSlots(dailyAppointments[currentDate.Date], timeSlots, doctor.Id);
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
                closestAppointments.Add(new Appointment(getLastId() + 1, slot, doctor.Id, patientId, ""));
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
                if (dailyAppointments.ContainsKey(currentDate.Date)) GetFreeDoctorsTimeSlots(dailyAppointments[currentDate.Date], timeSlots, appointmentRequest.Doctor.Id);
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
                if (freeTimeSlots[i].duration >= APPOINTMENT_DURATION)
                {
                    TimeSlot founded = MakeTimeSlotForPatient(freeTimeSlots[i], patientId);
                    if (founded == null) continue;
                    List<TimeSlot> tempTimeSlots = freeTimeSlots[i].Split(founded);
                    freeTimeSlots.Remove(freeTimeSlots[i]);
                    for (int j = 0; j < tempTimeSlots.Count; j++)
                    {
                        freeTimeSlots.Insert(i + j, tempTimeSlots[j]);
                    }
                    return founded;
                }
            }
            return null;
        }

        public TimeSlot MakeTimeSlotForPatient(TimeSlot timeSlot, int patientId)
        {
            Patient patient = Patient.getById(patientId);
            for (DateTime currentDate = timeSlot.start; currentDate <= timeSlot.start.AddMinutes(timeSlot.duration - 15); currentDate = currentDate.AddMinutes(1))
            {
                TimeSlot founded = new TimeSlot(currentDate, APPOINTMENT_DURATION);
                if (!patient.IsAvailable(founded)) continue;
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

        public List<Appointment> GetTodaysAppontments()
        {
            List<Appointment> todayAppointments = new List<Appointment>();
            foreach (Appointment appointment in appointments)
            {
                if (appointment.IsCanceled == false)
                {
                    if ((appointment.TimeSlot.start.ToShortDateString() == DateTime.Now.ToShortDateString()))
                    {
                        if (appointment.TimeSlot.start > DateTime.Now)
                        {
                            todayAppointments.Add(appointment);
                        }
                    }
                }
            }
            return todayAppointments;
        }
    }
}
