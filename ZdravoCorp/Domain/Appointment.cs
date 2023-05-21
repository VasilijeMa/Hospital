namespace ZdravoCorp.Domain
{
    public class Appointment
    {
        public int Id { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public bool IsCanceled { get; set; }
        public string IdRoom { get; set; }

        public Appointment(int id, TimeSlot timeSlot, int doctorId, int patientId, string idRoom)
        {
            Id = id;
            TimeSlot = timeSlot;
            DoctorId = doctorId;
            PatientId = patientId;
            IsCanceled = false;
            IdRoom = idRoom;
        }

        public Appointment() { }

        public Doctor getDoctor()
        {
            foreach (Doctor doctor in Singleton.Instance.doctors)
            {
                if (doctor.Id == DoctorId)
                {
                    return doctor;
                }
            }
            return null;
        }

        public Patient getPatient()
        {
            foreach (Patient patient in Singleton.Instance.patients)
            {
                if (patient.Id == PatientId)
                {
                    return patient;
                }
            }
            return null;
        }
    }
}
