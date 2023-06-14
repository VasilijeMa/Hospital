using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Model;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Repositories.Interfaces;

namespace ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
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
            File.WriteAllText("./../../../../ZdravoCorp/data/medicalRecords.json", json);
        }

        public List<MedicalRecord> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/medicalRecords.json");
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

        public List<MedicalRecord> GetMedicalRecords()
        {
            return records;
        }

        public void AddMedicalRecord(MedicalRecord medicalRecord)
        {
            records.Add(medicalRecord);
        }
    }
}
