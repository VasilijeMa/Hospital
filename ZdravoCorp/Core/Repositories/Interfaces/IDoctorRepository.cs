using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        public List<Doctor> LoadAll();

        public List<Doctor> GetDoctorBySpecialization(string specialization);

        public Doctor getDoctor(int doctorId);

        public Doctor GetByUsername(string username);

        public double GetAverageRating(Doctor doctor);

        public List<Doctor> SearchDoctors(string keyword);

        public bool Contains(string keyword, Doctor doctor);

        public List<Specialization> GetSpecializationOfDoctors();

        public List<Doctor> GetDoctors();
    }
}
