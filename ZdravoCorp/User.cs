using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    internal class User
    {
        //private string username;
        //private string password;

        public string Username { get; set; }

        public string Password { get; set; }

        public string Type { get; set; }

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
