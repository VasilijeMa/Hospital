using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;
using ZdravoCorp.ManagerView;

namespace ZdravoCorp.Core.Servieces
{
    public class UserService
    {
        private IUserRepository _userRepository;
        private IDoctorRepository _doctorRepository;
        private IPatientRepository _patientRepository;
        private INurseRepository _nurseRepository;

        public UserService()
        {
            _userRepository = Singleton.Instance.UserRepository;
            _doctorRepository = Singleton.Instance.DoctorRepository;
            _nurseRepository = Singleton.Instance.NurseRepository;
            _patientRepository = Singleton.Instance.PatientRepository;
        }

        public List<User> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        public void WriteAll()
        {
            _userRepository.WriteAll();
        }

        public void AddUser(User user)
        {
            _userRepository.AddUser(user);
        }

        public void RemoveUser(string username)
        {
            _userRepository.RemoveUser(username);
        }

        public void DisplayWindow(User user)
        {
            switch (user.Type)
            {
                case "doctor":
                    OpenDoctorWindow(user);
                    break;
                case "nurse":
                    OpenNurseWindow(user);
                    break;
                case "manager":
                    ManagerWindow managerWindow = new ManagerWindow();
                    managerWindow.ShowDialog();
                    break;
                case "patient":
                    OpenPatientWindow(user);
                    break;
                default:
                    break;
            }
        }

        private void OpenDoctorWindow(User user)
        {
            foreach (Doctor doctor in _doctorRepository.GetDoctors())
            {
                if (user.Username == doctor.Username)
                {
                    DoctorWindow doctorWindow = new DoctorWindow(doctor);
                    doctorWindow.ShowDialog();
                    break;
                }
            }
        }

        private void OpenNurseWindow(User user)
        {
            foreach (Nurse nurse in _nurseRepository.GetNurses())
            {
                if (user.Username == nurse.Username)
                {
                    NurseWindow nurseWindow = new NurseWindow(nurse);
                    nurseWindow.ShowDialog();
                    break;
                }
            }
        }

        private void OpenPatientWindow(User user)
        {
            Patient patient = _patientRepository.getByUsername(user.Username);
            if (!patient.IsBlocked)
            {
                PatientWindow patientWindow = new PatientWindow(patient);
                patientWindow.ShowDialog();
                if (patient.IsBlocked)
                {
                    WriteAll();
                }

                return;
            }

            MessageBox.Show("Your account is blocked.");
        }
    }
}
