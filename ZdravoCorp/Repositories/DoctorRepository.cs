using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class DoctorRepository
    {
        private List<Doctor> doctors;
        public List<Doctor> Doctors { get => doctors; }
        public DoctorRepository()
        {
            doctors = LoadAll();
        }
        public List<Doctor> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/doctor.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Doctor>>(json);
        }
        public List<Doctor> GetDoctorBySpecialization(string specialization)
        {
            return doctors.Where(doctor => doctor.Specialization.ToString() == specialization).ToList();
        }
        public Doctor getDoctor(int doctorId)
        {
            foreach (Doctor doctor in doctors)
            {
                if (doctor.Id == doctorId)
                {
                    return doctor;
                }
            }
            return null;
        }
    }
}
