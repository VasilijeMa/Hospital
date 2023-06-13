using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Repositories.Interfaces;
using ZdravoCorp.ManagerView;

namespace ZdravoCorp.Core.UserManager.Services
{
    public class UserService
    {
        private IUserRepository _userRepository;
        private IDoctorRepository _doctorRepository;
        private IPatientRepository _patientRepository;
        private INurseRepository _nurseRepository;

        public UserService()
        {
            _userRepository = Institution.Instance.UserRepository;
            _doctorRepository = Institution.Instance.DoctorRepository;
            _nurseRepository = Institution.Instance.NurseRepository;
            _patientRepository = Institution.Instance.PatientRepository;
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

        public List<User> GetNursesAndDoctors(string username)
        {
            return _userRepository.GetNursesAndDoctors(username);
        }
    }
}
