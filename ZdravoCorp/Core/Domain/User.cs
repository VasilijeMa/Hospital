using System.Windows;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.ManagerView;

namespace ZdravoCorp.Core.Domain
{
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Type { get; set; }

        public bool IsBlocked { get; set; }

        public User() { }

        public User(string username, string password, string type)
        {
            Username = username;
            Password = password;
            Type = type;
        }

        public override string ToString()
        {
            return "Username: " + Username + " Password: " + Password + "Type: " + Type;
        }
    }
}
