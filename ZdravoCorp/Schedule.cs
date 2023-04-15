using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class Schedule
    {
        public List<Appointment> appointments { get; set; }

        public Schedule()
        {
            this.appointments = LoadAllAppointments();
        }
        public Schedule(List<Appointment> appointments)
        {
            this.appointments = appointments;
        }

        public void CreateAppointment(TimeSlot timeSlot, Doctor doctor, Patient patient)
        {
            int id = getLastId() + 1;
            Appointment appointment = new Appointment(id ,timeSlot, doctor.Id, patient.Id);
            appointments.Add(appointment);
        }

        public bool IsAvailable(TimeSlot timeSlot, Doctor doctor)
        {
            foreach(Appointment appointment in appointments)
            {
                if(doctor.Id == appointment.DoctorId && appointment.TimeSlot.OverlapWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsAvailable(TimeSlot timeSlot, Patient patient)
        {
            foreach (Appointment appointment in appointments)
            {
                if (patient.Id == appointment.PatientId && appointment.TimeSlot.OverlapWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateAppointment()
        {

        }

        public void CancelAppointment(int appointmentId)
        {
            foreach (var appointment in appointments)
            {
                if(appointment.Id == appointmentId)
                {
                    appointment.IsCanceled = true;
                    break;
                }
            }
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
            return appointments.Max(appointment => appointment.Id);
        }
    }
}
