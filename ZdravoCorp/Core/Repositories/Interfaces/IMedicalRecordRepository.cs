using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
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
