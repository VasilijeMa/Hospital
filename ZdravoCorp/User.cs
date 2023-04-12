using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class User
    {
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
        public static List<User> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/users.json");
            var json = reader.ReadToEnd();
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            return users;
        }

        public override string ToString()
        {
            return "Username: " + Username + " Password: " + Password + "Type: " + Type;
        }

        public static void DisplayWindow(User user)
        {
            switch (user.Type)
            {
                case "doctor":
                    Doctor doctor = new Doctor();
             
                    foreach (Doctor oneDoctor in Singleton.Instance.doctors)
                    {
                        if (user.Username == oneDoctor.Username)
                        {
                            doctor = oneDoctor;
                        }
                    }

                    DoctorWindow doctorWindow = new DoctorWindow(doctor);
                    doctorWindow.Show();
                    break;

                case "nurse":
                    // code block
                    break;
                case "manager":

                    break;
                case "patient":

                    break;
                default:
                    // code block
                    break;
            }
        }
    }
}
