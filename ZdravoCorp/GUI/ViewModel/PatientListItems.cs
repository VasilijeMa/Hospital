using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.GUI.ViewModel
{
    public class PatientListItems
    {
        readonly Patient _patient;
        public int Id => _patient.Id;
        public string FirstName => _patient.FirstName;
        public string LastName => _patient.LastName;
        public int MedicalRecordId => _patient.MedicalRecordId;
        public DateOnly BirthDate => _patient.BirthDate;

        public PatientListItems(Patient patient)
        {
            _patient = patient;
        }

        public static implicit operator Patient(PatientListItems patient) => patient._patient;
        public static explicit operator PatientListItems(Patient d) => new PatientListItems(d);
    }
}
