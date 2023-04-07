using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Schedule
    {
        private List<Appointment> appointments;
        private List<Appointment> GetAppointments()
        {
            return appointments;
        }

        private void SetAppointments(List<Appointment> appointments)
        {
            this.appointments = appointments;
        }

        public Schedule(List<Appointment> appointments)
        {
            this.appointments = appointments;
        }

        public void CreateAppointment(TimeSlot timeSlot, Doctor doctor, Patient patient)
        {
            Appointment appointment = new Appointment(timeSlot, doctor, patient);
            appointments.Add(appointment);
        }

        public bool IsAvailable(TimeSlot timeSlot, Doctor doctor)
        {
            return appointments.Any(appointment => appointment.TimeSlot.OverlapWith(timeSlot) && doctor == appointment.GetDoctor());
        }

        public bool IsAvailable(TimeSlot timeSlot, Patient patient)
        {
            return appointments.Any(appointment => appointment.TimeSlot.OverlapWith(timeSlot) && patient == appointment.GetPatient());
        }

        public void UpdateAppointment()
        {

        }

        public void CancelAppointment()
        {

        }
    }
}
