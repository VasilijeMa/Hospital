using System.Collections.Generic;
using ZdravoCorp.Controllers;
using ZdravoCorp.Repositories;
using ZdravoCorp.Servieces;

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
            //Schedule = new Schedule();
            //Log = new Log();
            DoctorRepository doctorRepository = new DoctorRepository();
            doctors = doctorRepository.LoadAll();
            PatientRepository patientController = new PatientRepository();
            patients = patientController.LoadAll(); 
            //nurses = NurseRepository.LoadAll();
            MedicalRecordRepository medicalRecordRepository = new MedicalRecordRepository();
            medicalRecords = medicalRecordRepository.LoadAll();
            anamnesis = AnamnesisRepository.LoadAll();
            users = UserRepository.LoadAll();
            NotificarionAboutCancelledAppointmentRepository n = new NotificarionAboutCancelledAppointmentRepository();
            notificationAboutCancelledAppointment = n.LoadAll();
        }
    }
}
