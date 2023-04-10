using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Schedule
    {
        /*private List<Appointment> appointments;
        public List<Appointment> GetAppointments()
        {
            return appointments;
        }

        public void SetAppointments(List<Appointment> appointments)
        {
            this.appointments = appointments;
        }*/

        public List<Appointment> appointments { get; set; }

        public Schedule()
        {
            this.appointments = LoadAllAppointments();
        }
        public Schedule(List<Appointment> appointments)
        {
            this.appointments = appointments;
        }

        //public Schedule() { }

        public void CreateAppointment(TimeSlot timeSlot, Doctor doctor, Patient patient)
        {
            Appointment appointment = new Appointment(1,timeSlot, doctor.Id, patient.Id);
            appointments.Add(appointment);
        }

        public bool IsAvailable(TimeSlot timeSlot, Doctor doctor)
        {
            return appointments.Any(appointment => appointment.TimeSlot.OverlapWith(timeSlot) && doctor.Id == appointment.DoctorId);
        }

        public bool IsAvailable(TimeSlot timeSlot, Patient patient)
        {
            return appointments.Any(appointment => appointment.TimeSlot.OverlapWith(timeSlot) && patient.Id == appointment.PatientId);
        }

        public void UpdateAppointment()
        {

        }

        public void CancelAppointment()
        {

        }

        public List<Appointment> LoadAllAppointments()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/appointments.json");
            var json = reader.ReadToEnd();
            List<Appointment> appointments = JsonConvert.DeserializeObject<List<Appointment>>(json);
            return appointments;
        }
    }
}
