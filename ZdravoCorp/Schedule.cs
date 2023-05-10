using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
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
            List<Appointment> recommendedAppointments = new List<Appointment>();
            //kroz sve za trazenig doktora
            List<TimeSlot> closestTimeSlots = GetClosestTimeSlots(appointmentRequest);
            if (closestTimeSlots != null)
            {
                foreach (TimeSlot timeSlot in closestTimeSlots)
                {
                    int appointmentId = getLastId() + 1;
                    Appointment appointment = new Appointment(appointmentId, timeSlot, appointmentRequest.Doctor.Id, patientId);
                    recommendedAppointments.Add(appointment);
                }
                return recommendedAppointments;
            }
            MessageBox.Show("Nema koji zadovoljavaju sve parametra");
            //po prioritetu
            if (appointmentRequest.Priority == Priority.Doctor)
            {
                closestTimeSlots = GetClosestTimeSlotsByPriorityDoctor(appointmentRequest);
                if (closestTimeSlots != null)
                {
                    foreach (TimeSlot timeSlot in closestTimeSlots)
                    {
                        int appointmentId = getLastId() + 1;
                        Appointment appointment = new Appointment(appointmentId, timeSlot, appointmentRequest.Doctor.Id, patientId);
                        recommendedAppointments.Add(appointment);
                    }
                    return recommendedAppointments;
                }
            }
            else
            {
                recommendedAppointments = GetClosestAppointmentsByTimeInterval(appointmentRequest, patientId);
                if (recommendedAppointments != null)
                {
                    return recommendedAppointments;
                }
            }
            MessageBox.Show("Nema koji zadovoljavaju prioritet");
            //kroz sve dok ne nadje 3
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
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            DateTime endDate = appointmentRequest.LatestDate;
            DateTime startDate = DateTime.Now;
            int startHour = (int)appointmentRequest.EarliesTime.Hour;
            int startMinute = (int)appointmentRequest.EarliesTime.Minute;
            int endHour = (int)appointmentRequest.LatestTime.Hour;
            int endMinute = (int)appointmentRequest.LatestTime.Minute;
            for (DateTime i = startDate; i.Date <= endDate.Date; i = i.AddDays(1))
            {
                foreach (Doctor doctor in Singleton.Instance.doctors)
                {
                    if (doctor.Id == appointmentRequest.Doctor.Id) continue;
                    if (startDate == i && startDate.Hour > startHour)
                    {
                        if (startDate.Hour >= endHour && startDate.AddMinutes(15).Minute >= endMinute) continue;
                        startHour = i.AddMinutes(15).Hour;
                        startMinute = i.AddMinutes(15).Minute;
                    }
                    else
                    {
                        startHour = (int)appointmentRequest.EarliesTime.Hour;
                        startMinute = (int)appointmentRequest.EarliesTime.Minute;
                    }
                    DateTime startTime = new DateTime(i.Year, i.Month, i.Day, startHour, startMinute, 0);
                    DateTime endTime = new DateTime(i.Year, i.Month, i.Day, endHour, endMinute, 0);
                    int duration = (int)(endTime - startTime).Minutes;
                    timeSlots.Add(new TimeSlot(startTime, duration));
                    if (dailyAppointments.ContainsKey(i.Date))
                    {
                        GetFreeTimeSlots(dailyAppointments[i.Date], timeSlots, doctor.Id);
                    }
                    TimeSlot? timeSlot = GetFirstFreeTimeSlot(timeSlots);
                    if (timeSlot != null)
                    {
                        List<TimeSlot> tempTimeSlot = new List<TimeSlot>();
                        tempTimeSlot.Add(timeSlot);
                        List<Appointment> closestAppointments = new List<Appointment>();
                        foreach (var slot in tempTimeSlot)
                        {
                            int appointmentId = getLastId() + 1;
                            Appointment appointment = new Appointment(appointmentId, slot, doctor.Id, patientId);
                            closestAppointments.Add(appointment);
                        }
                        return closestAppointments;
                    }
                }
            }
            return null;
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
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            DateTime endDate = appointmentRequest.LatestDate;
            DateTime startDate = DateTime.Now;
            int startHour = (int)appointmentRequest.EarliesTime.Hour;
            int startMinute = (int)appointmentRequest.EarliesTime.Minute;
            int endHour = (int)appointmentRequest.LatestTime.Hour;
            int endMinute = (int)appointmentRequest.LatestTime.Minute;
            for (DateTime i = startDate; i.Date <= endDate; i = i.AddDays(1))
            {
                if (startDate == i && startDate.Hour >= startHour)
                {
                    if (startDate.Hour >= endHour && startDate.AddMinutes(15).Minute >= endMinute) continue;
                    startHour = i.AddMinutes(15).Hour;
                    startMinute = i.AddMinutes(15).Minute;
                }
                else
                {
                    startHour = (int)appointmentRequest.EarliesTime.Hour;
                    startMinute = (int)appointmentRequest.EarliesTime.Minute;
                }
                DateTime startTime = new DateTime(i.Year, i.Month, i.Day, startHour, startMinute, 0);
                DateTime endTime = new DateTime(i.Year, i.Month, i.Day, endHour, endMinute, 0);
                int duration = (int)(endTime - startTime).TotalMinutes;
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
            return null;
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
