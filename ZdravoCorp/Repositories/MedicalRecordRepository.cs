using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class MedicalRecordRepository
    {
        private List<MedicalRecord> records;
        public List<MedicalRecord> Records { get => records; }
        public MedicalRecordRepository()
        {
            records = LoadAll();
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
    }
}
