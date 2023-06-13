using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Repositories.Interfaces;

namespace ZdravoCorp.Core.UserManager.Repositories
{
    public class DoctorRepository : IDoctorRepository
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
            using StreamReader reader = new("./../../../../ZdravoCorp/data/doctor.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Doctor>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(doctors, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/doctor.json", json);
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

        public Doctor GetByUsername(string username)
        {
            foreach (Doctor doctor in doctors)
            {
                if (doctor.Username == username)
                {
                    return doctor;
                }
            }
            return null;
        }

        public double GetAverageRating(Doctor doctor)
        {
            return doctor.Ratings.Average();
        }

        public List<Doctor> SearchDoctors(string keyword)
        {
            return doctors.Where(doctor => Contains(keyword, doctor)).ToList();
        }

        public bool Contains(string keyword, Doctor doctor)
        {
            return doctor.FirstName.ToUpper().Contains(keyword) || doctor.LastName.ToUpper().Contains(keyword) || doctor.Specialization.ToString().ToUpper().Contains(keyword);
        }

        public List<Specialization> GetSpecializationOfDoctors()
        {
            List<Specialization> specializations = new List<Specialization>();
            foreach (var doctor in doctors)
            {
                if (!specializations.Contains(doctor.Specialization)) specializations.Add(doctor.Specialization);
            }

            return specializations;
        }

        public List<Doctor> GetDoctors()
        {
            return Doctors;
        }

        public void AddRating(int doctorId, int serviceQuality, int suggestToFriends)
        {
            getDoctor(doctorId).Ratings.Add(new List<int> { serviceQuality, suggestToFriends }.Average());
            WriteAll();
        }

        public List<string> GetFullNames()
        {
            List<string> doctorNames = new List<string>();
            foreach (Doctor doctor in doctors)
            {
                doctorNames.Add(doctor.FirstName + " " + doctor.LastName + " ID:" + doctor.Id.ToString());
            }
            return doctorNames;
        }
    }
}
