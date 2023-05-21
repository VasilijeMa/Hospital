using System.Collections.Generic;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Domain
{
    public class Singleton
    {
        private static Singleton instance;
        public Schedule Schedule { get; set; }
        public Log Log { get; set; }

        public List<Doctor> doctors;

        public List<Patient> patients;

        public List<Nurse> nurses;

        public List<MedicalRecord> medicalRecords;

        public List<User> users;

        public List<Anamnesis> anamnesis;

        public List<NotificationAboutCancelledAppointment> notificationAboutCancelledAppointment;
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    return instance = new Singleton();
                }
                return instance;
            }
        }
        private Singleton()
        {
            Schedule = new Schedule();
            Log = new Log();
            doctors = DoctorRepository.LoadAll();
            patients = PatientRepository.LoadAll();
            nurses = NurseRepository.LoadAll();
            medicalRecords = MedicalRecordRepository.LoadAll();
            anamnesis = AnamnesisRepository.LoadAll();
            users = UserRepository.LoadAll();
            notificationAboutCancelledAppointment = NotificarionAboutCancelledAppointmentRepository.LoadAll();
        }
    }
}
