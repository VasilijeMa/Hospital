using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Model;

namespace ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Repositories.Interfaces
{
    public interface IMedicalRecordRepository
    {
        public void WriteAll();

        public List<MedicalRecord> LoadAll();

        public MedicalRecord GetMedicalRecord(int medicalRecordId);

        public List<MedicalRecord> GetMedicalRecords();

        public void AddMedicalRecord(MedicalRecord medicalRecord);
    }
}
