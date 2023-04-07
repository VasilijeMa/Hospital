using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Appointment
    {
        private Doctor doctor;
        private Patient patient;
        public TimeSlot TimeSlot { get; set; }
        public Doctor GetDoctor() 
        {
            return doctor;
        }
        public void SetDoctor(Doctor doctor)
        {
            this.doctor = doctor;
        }
        public Patient GetPatient()
        {
            return patient; 
        }
        public void SetPatient(Patient patient)
        {
            this.patient = patient;
        }
        public Appointment(TimeSlot timeSlot, Doctor doctor, Patient patient)
        {
            TimeSlot = timeSlot;
            SetDoctor(doctor);
            SetPatient(patient);
        }
    }
}
