using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories
{
    public class MedicalRecordRepository
    {
        private List<MedicalRecord> records;
        public List<MedicalRecord> Records { get => records; }
        public MedicalRecordRepository()
        {
            records = LoadAll();
        }
        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(records, Formatting.Indented);
            File.WriteAllText("./../../../data/medicalRecords.json", json);
        }
        public List<MedicalRecord> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/medicalRecords.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<MedicalRecord>>(json);
        }
        public MedicalRecord GetMedicalRecord(int medicalRecordId)
        {
            foreach (MedicalRecord medicalRecord in Records)
            {
                if (medicalRecordId == medicalRecord.Id)
                {
                    return medicalRecord;
                }
            }
            return null;
        }

        public List<string> GetAllergyForPatient(int medicalRecordId)
        {
            MedicalRecord medicalRecord = GetMedicalRecord(medicalRecordId);
            List<string> allergies = medicalRecord.Allergens;
            allergies.ForEach(s => s = s.ToUpper());
            return allergies;
        }
    }
}
