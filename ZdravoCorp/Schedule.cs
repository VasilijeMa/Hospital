using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Ink;

namespace ZdravoCorp
{
    public class Schedule
    {
        private const int TIME_SLOT_TOLERANCE = 1;
        private const int APPOINTMENT_DURATION = 15;
        public List<Appointment> appointments { get; set; }
        public Dictionary<DateTime, List<Appointment>> dailyAppointments { get; set; }
        public Schedule()
        {
            this.appointments = LoadAllAppointments();
            CreateAppointmentsMap();
        }
        public Schedule(List<Appointment> appointments)
        {
            this.appointments = appointments;
        }

        public Appointment CreateAppointment(TimeSlot timeSlot, Doctor doctor, Patient patient)
        {
            int id = getLastId() + 1;
            Appointment appointment = new Appointment(id, timeSlot, doctor.Id, patient.Id);
            appointments.Add(appointment);
            return appointment;
        }

        public Appointment CreateAppointment(Appointment appointment)
        {
            appointments.Add(appointment);
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
            }
        }

        public List<Appointment> GetAppointmentsByRequest(AppointmentRequest appointmentRequest, int patientId)
        {
            List<TimeSlot> closestTimeSlots = GetClosestTimeSlots(appointmentRequest);
            if (closestTimeSlots != null) return GetAppointmentsFromTimeSlot(patientId, appointmentRequest.Doctor, closestTimeSlots[0]);
            //{
            //    foreach (TimeSlot timeSlot in closestTimeSlots)
            //    {
            //        recommendedAppointments.Add(new Appointment(getLastId() + 1, timeSlot, appointmentRequest.Doctor.Id, patientId));
            //    }
            //    return recommendedAppointments;
            //}
            if (appointmentRequest.Priority == Priority.Doctor)
            {
                closestTimeSlots = GetClosestTimeSlotsByPriorityDoctor(appointmentRequest);
                if (closestTimeSlots != null) return GetAppointmentsFromTimeSlot(patientId, appointmentRequest.Doctor, closestTimeSlots[0]);
                //{
                //    foreach (TimeSlot timeSlot in closestTimeSlots)
                //    {
                //        recommendedAppointments.Add(new Appointment(getLastId() + 1, timeSlot, appointmentRequest.Doctor.Id, patientId));
                //    }
                //    return recommendedAppointments;
                //}
            }
            else
            {
                List<Appointment> recommendedAppointments = GetClosestAppointmentsByTimeInterval(appointmentRequest, patientId);
                if (recommendedAppointments != null) return recommendedAppointments;
            }
            return GetClosestAppointments(appointmentRequest, patientId);
        }

        public List<Appointment> GetClosestAppointments(AppointmentRequest appointmentRequest, int patientId)
        {
            List<Appointment> closestAppointments = new List<Appointment>();
            for (DateTime i = DateTime.Now.AddMinutes(15); ; i = i.AddMinutes(1))
            {
                if (closestAppointments.Count() == 3) break;
                foreach (Doctor doctor in Singleton.Instance.doctors)
                {
                    TimeSlot freeTimeSlot = new TimeSlot(i, APPOINTMENT_DURATION);
                    if (doctor.IsAvailable(freeTimeSlot))
                    {
                        if (!AppointmentTimeOverlaps(closestAppointments, freeTimeSlot, doctor.Id)) continue;
                        Appointment appointment = new Appointment(getLastId() + 1, freeTimeSlot, doctor.Id, patientId);
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
                if (appointment.TimeSlot.OverlapWith(timeSlot) && appointment.DoctorId == doctorId) return false;
            }
            return true;
        }

        public List<Appointment> GetClosestAppointmentsByTimeInterval(AppointmentRequest appointmentRequest, int patientId)
        {
            //DateTime endDate = appointmentRequest.LatestDate;
            //DateTime startDate = DateTime.Now;
            //int startHour = (int)appointmentRequest.EarliesTime.Hour;
            //int startMinute = (int)appointmentRequest.EarliesTime.Minute;
            //int endHour = (int)appointmentRequest.LatestTime.Hour;
            //int endMinute = (int)appointmentRequest.LatestTime.Minute;
            for (DateTime currentDate = DateTime.Now; currentDate.Date <= appointmentRequest.LatestDate.Date; currentDate = currentDate.AddDays(1))
            {
                foreach (Doctor doctor in Singleton.Instance.doctors)
                {
                    if (doctor.Id == appointmentRequest.Doctor.Id) continue;
                    //if (startDate == i && startDate.Hour > startHour)
                    //{
                    //    if (startDate.Hour >= endHour && startDate.AddMinutes(15).Minute >= endMinute) continue;
                    //    startHour = i.AddMinutes(15).Hour;
                    //    startMinute = i.AddMinutes(15).Minute;
                    //}
                    //else
                    //{
                    //    startHour = (int)appointmentRequest.EarliesTime.Hour;
                    //    startMinute = (int)appointmentRequest.EarliesTime.Minute;
                    //}
                    DateTime startTime = GetStartTime(currentDate, appointmentRequest.EarliesTime); /*new DateTime(i.Year, i.Month, i.Day, startHour, startMinute, 0);*/
                    DateTime endTime = GetEndTime(currentDate, appointmentRequest.LatestTime); /* new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, endHour, endMinute, 0);*/
                    int duration = (int)(endTime - startTime).Minutes;
                    List<TimeSlot> timeSlots = new List<TimeSlot>() { new TimeSlot(startTime, duration) };
                    //timeSlots.Add(new TimeSlot(startTime, duration));
                    if (dailyAppointments.ContainsKey(currentDate.Date)) GetFreeDoctorsTimeSlots(dailyAppointments[currentDate.Date], timeSlots, doctor.Id);
                    TimeSlot? timeSlot = GetFirstFreeTimeSlot(timeSlots);
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
                closestAppointments.Add(new Appointment(getLastId() + 1, slot, doctor.Id, patientId));
            }
            return closestAppointments;
        }

        public List<TimeSlot> GetClosestTimeSlotsByPriorityDoctor(AppointmentRequest appointmentRequest)
        {
            appointmentRequest.LatestDate = appointmentRequest.LatestDate.AddDays(TIME_SLOT_TOLERANCE);
            appointmentRequest.EarliesTime = new TimeOnly(0, 0);
            appointmentRequest.LatestTime = new TimeOnly(23, 59);
            return GetClosestTimeSlots(appointmentRequest);
        }

        public List<TimeSlot> GetClosestTimeSlots(AppointmentRequest appointmentRequest)
        {
            //List<TimeSlot> timeSlots = new List<TimeSlot>();
            //DateTime startDate = DateTime.Now;
            //DateTime endDate = appointmentRequest.LatestDate;
            for (DateTime currentDate = DateTime.Now/*startDate*/; currentDate.Date <= appointmentRequest.LatestDate/*endDate*/; currentDate = currentDate.AddDays(1))
            {
                //int startHour = (int)appointmentRequest.EarliesTime.Hour;
                //int startMinute = (int)appointmentRequest.EarliesTime.Minute;
                //if (startDate == currentDate && startDate.Hour >= startHour)
                //{
                //    if (startDate.Hour >= endHour && startDate.AddMinutes(15).Minute >= endMinute) continue;
                //    startHour = currentDate.AddMinutes(15).Hour;
                //    startMinute = currentDate.AddMinutes(15).Minute;
                //}
                //else
                //{
                //    startHour = (int)appointmentRequest.EarliesTime.Hour;
                //    startMinute = (int)appointmentRequest.EarliesTime.Minute;
                //}
                DateTime startTime = GetStartTime(currentDate, appointmentRequest.EarliesTime); /*new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, startHour, startMinute, 0);*/
                //int endHour = (int)appointmentRequest.LatestTime.Hour;
                //int endMinute = (int)appointmentRequest.LatestTime.Minute;
                DateTime endTime = GetEndTime(currentDate, appointmentRequest.LatestTime); /*new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, endHour, endMinute, 0);*/
                int duration = (int)(endTime - startTime).TotalMinutes;
                List<TimeSlot> timeSlots = new List<TimeSlot>() { new TimeSlot(startTime, duration) };
                //timeSlots.Add(new TimeSlot(startTime, duration));
                if (dailyAppointments.ContainsKey(currentDate.Date)) GetFreeDoctorsTimeSlots(dailyAppointments[currentDate.Date], timeSlots, appointmentRequest.Doctor.Id);
                TimeSlot? timeSlot = GetFirstFreeTimeSlot(timeSlots);
                if (timeSlot != null) return new List<TimeSlot> { timeSlot };
                //{
                //    List<TimeSlot> tempTimeSlot = new List<TimeSlot>();
                //    tempTimeSlot.Add(timeSlot);
                //    return tempTimeSlot;
                //}
            }
            return null;
        }

        private DateTime GetStartTime(DateTime currentDate, TimeOnly earliestTime)
        {
            DateTime startTime = new DateTime();
            //int startHour = (int)appointmentRequest.EarliesTime.Hour;
            //int startMinute = (int)appointmentRequest.EarliesTime.Minute;
            if (currentDate.Date == DateTime.Now.Date && DateTime.Now.TimeOfDay >= earliestTime.ToTimeSpan())
            {
                //if (DateTime.Now.Hour >= endHour && DateTime.Now.AddMinutes(15).Minute >= endMinute) return;
                startTime = currentDate.Date.AddMinutes(15);
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

        public TimeSlot? GetFirstFreeTimeSlot(List<TimeSlot> freeTimeSlots)
        {
            for (int i = 0; i < freeTimeSlots.Count; i++)
            {
                if (freeTimeSlots[i].duration >= APPOINTMENT_DURATION)
                {
                    TimeSlot founded = new TimeSlot(freeTimeSlots[i].start, APPOINTMENT_DURATION);
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

        public void GetFreeDoctorsTimeSlots(List<Appointment> appointments, List<TimeSlot> timeSlots, int doctorId)
        {
            foreach (Appointment appointment in appointments)
            {
                if (doctorId != appointment.DoctorId) continue;
                SplitTimeSlot(appointment, timeSlots);
            }
        }

        public void SplitTimeSlot(Appointment appointment, List<TimeSlot> timeSlots)
        {
            for (int i = 0; i < timeSlots.Count; i++)
            {
                //tempTimeSlots = new List<TimeSlot>();
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

    }
}
