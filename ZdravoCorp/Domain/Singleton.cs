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
            _logRepository = new LogRepository();
            _doctorRepository = new DoctorRepository();
            _patientRepository = new PatientRepository();
            _nurseRepository = new NurseRepository();
            _medicalRecordRepository = new MedicalRecordRepository();
            _anamnesisRepository = new AnamnesisRepository();
            _userRepository = new UserRepository();
            _notificationAboutCancelledAppointmentRepository = new NotificationAboutCancelledAppointmentRepository();
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
