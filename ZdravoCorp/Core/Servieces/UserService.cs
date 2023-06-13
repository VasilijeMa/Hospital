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

        public List<User> GetNursesAndDoctors(string username)
        {
            return _userRepository.GetNursesAndDoctors(username);
        }
    }
}
