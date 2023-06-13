using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.UserManager.Model;

namespace ZdravoCorp.Core.UserManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public List<User> LoadAll();

        public void WriteAll();

        public void RemoveUser(string username);

        public List<User> GetUsers();

        public void AddUser(User user);

        public List<User> GetNursesAndDoctors(string username);
    }
}
