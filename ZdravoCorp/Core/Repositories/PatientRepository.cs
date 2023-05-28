﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories
{
    public class PatientRepository
    {
        private List<Patient> patients;
        public List<Patient> Patients { get => patients; }
        public PatientRepository()
        {
            patients = LoadAll();
        }
        public List<Patient> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/patient.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Patient>>(json);
        }
        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(patients, Formatting.Indented);
            File.WriteAllText("./../../../data/patient.json", json);
        }
        public Patient getPatient(int patientId)
        {
            foreach (Patient patient in patients)
            {
                if (patient.Id == patientId)
                {
                    return patient;
                }
            }
            return null;
        }
        public Patient getById(int id)
        {
            foreach (Patient patient in patients)
            {
                if (patient.Id == id)
                {
                    return patient;
                }
            }
            return null;
        }
        public Patient getByUsername(string username)
        {
            foreach (Patient patient in patients)
            {
                if (patient.Username == username)
                {
                    return patient;
                }
            }
            return null;
        }

        public int GetMedicalRecordId(int patientId)
        {
            foreach (var patient in patients)
            {
                if (patient.Id == patientId)
                {
                    return patient.MedicalRecordId;
                }
            }
            return -1;
        }
    }
}
