﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZdravoCorp
{
    public class Schedule
    {
        public List<Appointment> appointments { get; set; }
        public Dictionary<DateTime, List<Appointment>> dailyAppointments {  get; set; }
        public Schedule()
        {
            this.appointments = LoadAllAppointments();
            CreateAppointmentMap();
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

        public void CreateAppointmentMap()
        {
            dailyAppointments = new Dictionary<DateTime, List<Appointment>>();
            foreach(var appointment in appointments)
            {
                if(dailyAppointments.ContainsKey(appointment.TimeSlot.start.Date))
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
