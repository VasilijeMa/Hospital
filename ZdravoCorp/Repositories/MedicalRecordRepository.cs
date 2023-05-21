using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class MedicalRecordRepository
    {
        private List<MedicalRecord> Records { get; set; }

        public MedicalRecordRepository()
        {
            LoadAll();
        }
        public void WriteAll(List<MedicalRecord> newlistofrecords)
        {
            string json = JsonConvert.SerializeObject(newlistofrecords, Formatting.Indented);
            File.WriteAllText("./../../../data/medicalRecords.json", json);
        }
        public List<MedicalRecord> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/medicalRecords.json");
            var json = reader.ReadToEnd();
            Records = JsonConvert.DeserializeObject<List<MedicalRecord>>(json);
            return Records;
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
    }
}
