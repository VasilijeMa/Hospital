using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class Appointment
    {
        public int Id { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public bool IsCanceled { get; set; }
        Anamnesis anamnesis  { get; set; }

        private Examination examination;

        public Appointment(int id, TimeSlot timeSlot, int doctorId, int patientId)
        {
            Id = id;
            TimeSlot = timeSlot;
            DoctorId = doctorId;
            PatientId = patientId;
            IsCanceled = false;
        }

        public Appointment() { }

        /*public void startAppointment()
        {
            Patient patient = Singleton.Instance.patients[PatientId];
            //CreateMedicalRecordWindow medicalRecord = CreateMedicalRecordWindow();
        }*/

        public Patient getPatient()
        {
            foreach (Patient patient in Singleton.Instance.patients)
            {
                if (patient.Id == this.PatientId)
                {
                    return patient;
                }
            }
            return null;
        }

        public bool isAbleToStart()
        {
            DateTime earliestStart = TimeSlot.start.Add(new TimeSpan(0, -15, 0));
            DateTime latestStart = TimeSlot.start.Add(new TimeSpan(0, TimeSlot.duration, 0));

            if (DateTime.Now < earliestStart || DateTime.Now > latestStart)
            {
                return false;
            }
            return true;
        }
    }
}
