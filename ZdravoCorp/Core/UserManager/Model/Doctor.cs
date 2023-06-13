using System.Collections.Generic;
using ZdravoCorp.Core.Enums;

namespace ZdravoCorp.Core.UserManager.Model
{
    public class Doctor : User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Specialization Specialization { get; set; }
        public List<double> Ratings { get; set; }

        public Doctor() : base() { }

        public Doctor(int id, string name, string lastname, Specialization specialization, string username, string password, string type) : base(username, password, type)
        {
            Id = id;
            FirstName = name;
            LastName = lastname;
            Specialization = specialization;
        }

        public override string ToString()
        {
            return "Id: " + Id + ", " + FirstName + " " + LastName + ", " + Specialization;
        }
    }
}
