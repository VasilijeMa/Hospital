using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Domain.Enums;
using ZdravoCorp.Servieces;

namespace ZdravoCorp.Domain
{
    public class Doctor : User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Specialization Specialization { get; set; }

        public Doctor() : base() { }

        public Doctor(int id, string name, string lastname, Specialization specialization, string username, string password, string type) : base(username, password, type)
        {
            Id = id;
            FirstName = name;
            LastName = lastname;
            Specialization = specialization;
        }
    }
}
