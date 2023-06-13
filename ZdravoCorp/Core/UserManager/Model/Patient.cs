using System;

namespace ZdravoCorp.Core.UserManager.Model
{
    public class Patient : User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public int MedicalRecordId { get; set; }
        public bool IsBlocked { get; set; }

        public Patient(int id, string firstName, string lastName, DateOnly birthDate, int medicalRecordId, string username, string password, string type, bool isBlocked) : base(username, password, type)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            MedicalRecordId = medicalRecordId;
            IsBlocked = isBlocked;
        }

        public Patient() : base() { }

        public override string ToString()
        {
            return "Id: " + Id + ". " + FirstName + " " + LastName;
        }

    }
}
