using ZdravoCorp.Core.CommunicationSystem.Repositories;
using ZdravoCorp.Core.CommunicationSystem.Repositories.Interfaces;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Repositories;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Repositories.Interfaces;
using ZdravoCorp.Core.PatientNotification.Repositories;
using ZdravoCorp.Core.PatientNotification.Repositories.Interfaces;
using ZdravoCorp.Core.PatientSatisfaction.Repositories;
using ZdravoCorp.Core.PatientSatisfaction.Repositories.Interfaces;
using ZdravoCorp.Core.PhysicalAssets.Repositories;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;
using ZdravoCorp.Core.Scheduling.Repositories;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;
using ZdravoCorp.Core.VacationRequest.Repositories;
using ZdravoCorp.Core.VacationRequest.Repositories.Interfaces;

namespace ZdravoCorp.Core.Domain
{
    public class Singleton
    {
        private readonly IAnamnesisRepository _anamnesisRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ILogRepository _logRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly INotificationAboutCancelledAppointmentRepository _notificationAboutCancelledAppointmentRepository;
        private readonly INurseRepository _nurseRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IExaminationRepository _examinationRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMedicamentRepository _medicamentRepository;
        private readonly IMedicamentToAddRepository _medicamentToAddRepository;
        private readonly IFreeDaysRepository _freeDaysRepository;
        private readonly IHospitalSurveyRepository _hospitalSurveyRepository;
        private readonly IDoctorSurveyRepository _doctorSurveyRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IHospitalStayRepository _hospitalStayRepository;

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
            _examinationRepository = new ExaminationRepository();
            _notificationRepository = new NotificationRepository();
            _medicamentRepository = new MedicamentRepository();
            _medicamentToAddRepository = new MedicamentToAddRepository();
            _freeDaysRepository = new FreeDaysRepository();
            _hospitalSurveyRepository = new HospitalSurveyRepository();
            _doctorSurveyRepository = new DoctorSurveyRepository();
            _chatRepository = new ChatsRepository();
            _roomRepository = new RoomRepository();
            _hospitalStayRepository = new HospitalStayRepository();
        }

        public IAnamnesisRepository AnamnesisRepository { get => _anamnesisRepository; }

        public IDoctorRepository DoctorRepository { get => _doctorRepository; }

        public ILogRepository LogRepository { get => _logRepository; }

        public IMedicalRecordRepository MedicalRecordRepository { get => _medicalRecordRepository; }

        public INotificationAboutCancelledAppointmentRepository NotificationAboutCancelledAppointmentRepository { get => _notificationAboutCancelledAppointmentRepository; }

        public INurseRepository NurseRepository { get => _nurseRepository; }

        public IPatientRepository PatientRepository { get => _patientRepository; }

        public IScheduleRepository ScheduleRepository { get => _scheduleRepository; }

        public IUserRepository UserRepository { get => _userRepository; }

        public IExaminationRepository ExaminationRepository { get => _examinationRepository; }

        public INotificationRepository NotificationRepository { get => _notificationRepository; }

        public IMedicamentToAddRepository MedicamentToAddRepository { get => _medicamentToAddRepository; }

        public IMedicamentRepository MedicamentRepository { get => _medicamentRepository; }

        public IFreeDaysRepository FreeDaysRepository { get => _freeDaysRepository; }

        public IHospitalSurveyRepository HospitalSurveyRepository { get => _hospitalSurveyRepository;}

        public IDoctorSurveyRepository DoctorSurveyRepository { get => _doctorSurveyRepository; }

        public IChatRepository ChatRepository { get => _chatRepository; }

        public IHospitalStayRepository HospitalStayRepository { get => _hospitalStayRepository; }

        public IRoomRepository RoomRepository { get => _roomRepository; }
    }
}
