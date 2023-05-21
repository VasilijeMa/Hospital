using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class PatientRepository
    {
        public static List<Patient> patients;
        public PatientRepository()
        {
            LoadAll();
        }
        public List<Patient> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/patient.json");
            var json = reader.ReadToEnd();
            patients = JsonConvert.DeserializeObject<List<Patient>>(json);
            return patients;
        }
        public void WriteAll(List<Patient> newlistofpatients)
        {
            string json = JsonConvert.SerializeObject(newlistofpatients, Formatting.Indented);
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
    }
}
