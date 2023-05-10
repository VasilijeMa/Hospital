using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public List<Appointment> GetAppointmentsByRequest(AppointmentRequest appointmentRequest)
        {
            List<Appointment> appointments = new List<Appointment>();
            //kroz sve za trazenig doktora

            //po prioritetu

            //kroz sve dok ne nadje 3

            return null;
        }

        public List<TimeSlot> GetClosestTimeSlots(AppointmentRequest appointmentRequest)
        {
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            DateTime endDate = appointmentRequest.LatestDate;
            DateTime startDate = DateTime.Now;
            int startHour = (int)appointmentRequest.EarliestHour.Hour;
            int startMinute = (int)appointmentRequest.EarliestHour.Minute;
            int endHour = (int)appointmentRequest.LatestHour.Hour;
            int endMinute = (int)appointmentRequest.LatestHour.Minute;
            if (appointmentRequest.Priority == Priority.Doctor)
            {
                endDate = endDate.AddDays(TIME_SLOT_TOLERANCE);
                //startHour = 0;
                //startMinute = 0;
                //endHour = 0;
                //endMinute = 0;
            }
            for (DateTime i = startDate; i <= endDate; i = i.AddDays(1))
            {
                if (startDate == i && startDate.Hour > startHour)
                {
                    if (startDate.Hour >= endHour && startDate.Minute >= endMinute) continue;
                    startHour = i.Hour;
                    startMinute = i.Minute;
                }
                else
                {
                    startHour = (int)appointmentRequest.EarliestHour.Hour;
                    startMinute = (int)appointmentRequest.EarliestHour.Minute;
                }
                DateTime startTime = new DateTime(i.Year, i.Month, i.Day, startHour, startMinute, 0);
                DateTime endTime = new DateTime(i.Year, i.Month, i.Day, endHour, endMinute, 0);
                int duration = (int)(endTime - startTime).TotalMinutes;
                //if (i >= endTime) continue;
                //else if (i >= startTime)
                //{
                //    startTime = i.Hour;
                //}
                timeSlots = new List<TimeSlot>();
                timeSlots.Add(new TimeSlot(startTime, duration));
                if (dailyAppointments.ContainsKey(i.Date))
                {
                    GetFreeTimeSlots(dailyAppointments[i.Date], timeSlots, appointmentRequest.Doctor.Id);
                }
                TimeSlot? timeSlot = GetFirstFreeTimeSlot(timeSlots);
                if (timeSlot != null)
                {
                    List<TimeSlot> tempTimeSlot = new List<TimeSlot>();
                    tempTimeSlot.Add(timeSlot);
                    return tempTimeSlot;
                }
            }

            if (appointmentRequest.Priority == Priority.TimeSlot)
            {
                foreach (Doctor doctor in Singleton.Instance.doctors)
                {
                    if (doctor.Id == appointmentRequest.Doctor.Id) continue;
                    for (DateTime i = startDate; i <= endDate; i = i.AddDays(1))
                    {
                        DateTime startTime = new DateTime(i.Year, i.Month, i.Day, startHour, startMinute, 0);
                        DateTime endTime = new DateTime(i.Year, i.Month, i.Day, endHour, endMinute, 0);
                        int duration = (int)(endTime - startTime).Minutes;
                        timeSlots.Add(new TimeSlot(startDate, duration));
                        if (dailyAppointments.ContainsKey(i.Date))
                        {
                            GetFreeTimeSlots(dailyAppointments[i.Date], timeSlots, doctor.Id);
                        }
                        TimeSlot? timeSlot = GetFirstFreeTimeSlot(timeSlots);
                        if (timeSlot != null)
                        {
                            List<TimeSlot> tempTimeSlot = new List<TimeSlot>();
                            tempTimeSlot.Add(timeSlot);
                            return tempTimeSlot;
                        }
                    }
                }
            }
            List<TimeSlot> freeTimeSlots = new List<TimeSlot>();
            for (DateTime i = DateTime.Now; ; i = i.AddMinutes(1))
            {
                if (freeTimeSlots.Count() == 3) break;
                foreach (Doctor doctor in Singleton.Instance.doctors)
                {
                    TimeSlot freeTimeSlot = new TimeSlot(i, APPOINTMENT_DURATION);
                    if (doctor.IsAvailable(freeTimeSlot))
                    {
                        freeTimeSlots.Add(freeTimeSlot);
                        if (freeTimeSlots.Count() == 3) break;
                    }
                }
            }
            return freeTimeSlots;
        }

        public TimeSlot? GetFirstFreeTimeSlot(List<TimeSlot> timeSlots)
        {
            for (int i = 0; i < timeSlots.Count; i++)
            {
                if (timeSlots[i].duration >= APPOINTMENT_DURATION)
                {
                    TimeSlot founded = new TimeSlot(timeSlots[i].start, 15);
                    List<TimeSlot> tempTimeSlots = timeSlots[i].Split(founded);
                    timeSlots.Remove(timeSlots[i]);
                    for (int j = 0; j < tempTimeSlots.Count; j++)
                    {
                        timeSlots.Insert(i + j, tempTimeSlots[j]);
                    }
                    return founded;
                }
            }
            return null;
        }

        //proveriti -1
        public void GetFreeTimeSlots(List<Appointment> appointments, List<TimeSlot> timeSlots, int doctorId)
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
                List<TimeSlot> tempTimeSlots = new List<TimeSlot>();
                if (timeSlots[i].OverlapWith(appointment.TimeSlot))
                {
                    tempTimeSlots = timeSlots[i].Split(appointment.TimeSlot);
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
