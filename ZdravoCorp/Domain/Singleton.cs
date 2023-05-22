using System.Collections.Generic;
using ZdravoCorp.Controllers;
using ZdravoCorp.Repositories;
using ZdravoCorp.Servieces;

namespace ZdravoCorp.Domain
{
    public class Singleton
    {
        private readonly AnamnesisRepository _anamnesisRepository;
        private readonly DoctorRepository _doctorRepository;
        private readonly LogRepository _logRepository;
        private readonly MedicalRecordRepository _medicalRecordRepository;
        private readonly NotificationAboutCancelledAppointmentRepository _notificationAboutCancelledAppointmentRepository;
        private readonly NurseRepository _nurseRepository;
        private readonly PatientRepository _patientRepository;
        private readonly ScheduleRepository _scheduleRepository;
        private readonly UserRepository _userRepository;

        private static Singleton instance;
        //public Schedule Schedule { get; set; }
        //public Log Log { get; set; }

        //public List<Doctor> doctors;

        //public List<Patient> patients;

        //public List<Nurse> nurses;

        //public List<MedicalRecord> medicalRecords;

        //public List<User> users;

        //public List<Anamnesis> anamnesis;

        //public List<NotificationAboutCancelledAppointment> notificationAboutCancelledAppointment;
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
            _scheduleRepository = new ScheduleRepository();
            //Schedule = _scheduleRepository.Schedule;
            _logRepository = new LogRepository();
            //Log = _logRepository.Log;
            _doctorRepository = new DoctorRepository();
            //doctors = _doctorRepository.LoadAll();
            _patientRepository = new PatientRepository();
            //patients = _patientRepository.LoadAll();
            _nurseRepository = new NurseRepository();
            //nurses = _nurseRepository.Nurses;
            _medicalRecordRepository = new MedicalRecordRepository();
            //medicalRecords = _medicalRecordRepository.LoadAll();
            _anamnesisRepository = new AnamnesisRepository();
            //anamnesis = _anamnesisRepository.Anamneses;
            _userRepository = new UserRepository();
            //users = _userRepository.Users;
            _notificationAboutCancelledAppointmentRepository = new NotificationAboutCancelledAppointmentRepository();
            //notificationAboutCancelledAppointment = _notificationAboutCancelledAppointmentRepository.LoadAll();
        }

        public AnamnesisRepository AnamnesisRepository { get => _anamnesisRepository; }
        public DoctorRepository DoctorRepository { get => _doctorRepository; }
        public LogRepository LogRepository { get => _logRepository; }
        public MedicalRecordRepository MedicalRecordRepository { get => _medicalRecordRepository; }
        public NotificationAboutCancelledAppointmentRepository NotificationAboutCancelledAppointmentRepository { get => _notificationAboutCancelledAppointmentRepository; }
        public NurseRepository NurseRepository { get => _nurseRepository; }
        public PatientRepository PatientRepository { get => _patientRepository; }
        public ScheduleRepository ScheduleRepository { get => _scheduleRepository; }
        public UserRepository UserRepository { get => _userRepository; }
    }
}
