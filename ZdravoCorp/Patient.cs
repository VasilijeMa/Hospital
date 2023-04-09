using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        private int MedicalRecordId { get; set; }
        public Patient(int id, string firstName, string lastName, DateOnly birthDate, int medicalRecordId)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthDate = birthDate;
            this.MedicalRecordId = medicalRecordId;
        }
        public Patient() { }

    }
}
