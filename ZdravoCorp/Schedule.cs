using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    public class Schedule
    {
        public List<Appointment> todaysAppointments { get; set; } 
        public List<Appointment> appointments { get; set; }
        public Schedule()
        {
            this.appointments = LoadAllAppointments();
            this.todaysAppointments = GetTodaysAppontments();
        }

        public Schedule(List<Appointment> appointments)
        {
            this.appointments = appointments;
        }

        public Appointment CreateAppointment(TimeSlot timeSlot, Doctor doctor, Patient patient)
        {
            string roomId = Appointment.takeRoom(timeSlot);
            if (roomId == "")
            {
                MessageBox.Show("All rooms are full.");
                return null;
            }
            int id = getLastId() + 1;
            Appointment appointment = new Appointment(id, timeSlot, doctor.Id, patient.Id, roomId);
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

        public List<Appointment> GetTodaysAppontments() {
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
