using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.GUI.ViewModel
{
    public class DoctorListItem
    {
        readonly Doctor _doctor;
        public int Id => _doctor.Id;
        public string FirstName => _doctor.FirstName;
        public string LastName => _doctor.LastName;
        public string Specialization => _doctor.Specialization.ToString();
        public double Rating => _doctor.Ratings.Average();

        public DoctorListItem(Doctor doctor)
        {
            _doctor = doctor;
        }

        public static implicit operator Doctor(DoctorListItem d) => d._doctor;
        public static explicit operator DoctorListItem(Doctor d) => new DoctorListItem(d);
    }
}
