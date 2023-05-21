using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class PatientRepository
    {
        public static List<Patient> Patients { get; set; }
        public static List<Patient> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/patient.json");
            var json = reader.ReadToEnd();
            List<Patient> patients = JsonConvert.DeserializeObject<List<Patient>>(json);
            Patients = patients;
            return patients;
        }

        public static void WriteAll(List<Patient> newlistofpatients)
        {
            string json = JsonConvert.SerializeObject(newlistofpatients, Formatting.Indented);
            File.WriteAllText("./../../../data/patient.json", json);
        }

    }
}
