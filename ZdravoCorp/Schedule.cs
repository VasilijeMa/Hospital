using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ZdravoCorp
{
    public class Schedule
    {
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

        public List<TimeSlot> GetClosestTimeSlots(AppointmentRequest appointmentRequest)
        {
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            for (DateTime i = DateTime.Now; i <= appointmentRequest.LatestDate; i = i.AddDays(1))
            {
                DateTime startDate = new DateTime(i.Year, i.Month, i.Day, (int)appointmentRequest.EarliestHour.Hour, (int)appointmentRequest.EarliestHour.Minute, 0);
                DateTime endDate = new DateTime(i.Year, i.Month, i.Day, (int)appointmentRequest.LatestHour.Hour, (int)appointmentRequest.LatestHour.Minute, 0);
                int duration = (int)(endDate - startDate).Minutes;
                timeSlots.Add(new TimeSlot(startDate, duration));
                if (dailyAppointments.ContainsKey(i.Date))
                {
                    GetFreeTimeSlots(dailyAppointments[i.Date], timeSlots, appointmentRequest.DoctorId);
                }
                //GetFirstFreeTimeSlot
                //nalazi prvi timeslot koji ima slobodnih 15min 
                //ako postoji zakazi? ako ne dalje, ako nema uopste 3 najbliza, prioritet
            }
            return timeSlots;
        }

        public TimeSlot GetFirstFreeTimeSlot(List<TimeSlot> timeSlots)
        {
            foreach(TimeSlot timeSlot in timeSlots)
            {
                if (timeSlot.duration >= 15)
                {
                    return timeSlot;
                }
            }
            return null;
        }

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

        //public bool IsAvailable(TimeSlot timeSlot, Doctor doctor, int appointmentId = -1)
        //{
        //    foreach (Appointment appointment in appointments)
        //    {
        //        if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
        //        if (doctor.Id == appointment.DoctorId && appointment.TimeSlot.OverlapWith(timeSlot))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        //public bool IsAvailable(TimeSlot timeSlot, Patient patient, int appointmentId = -1)
        //{
        //    foreach (Appointment appointment in appointments)
        //    {
        //        if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
        //        if (patient.Id == appointment.PatientId && appointment.TimeSlot.OverlapWith(timeSlot))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}
