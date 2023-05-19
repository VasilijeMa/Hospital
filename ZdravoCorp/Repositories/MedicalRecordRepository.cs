using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Repositories
{
    public class MedicalRecordRepository
    {
        public static void WriteAll(List<MedicalRecord> newlistofrecords)
        {
            string json = JsonConvert.SerializeObject(newlistofrecords, Formatting.Indented);
            File.WriteAllText("./../../../data/medicalRecords.json", json);
        }
        public static List<MedicalRecord> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/medicalRecords.json");
            var json = reader.ReadToEnd();
            List<MedicalRecord> records = JsonConvert.DeserializeObject<List<MedicalRecord>>(json);
            return records;
        }
    }
}
