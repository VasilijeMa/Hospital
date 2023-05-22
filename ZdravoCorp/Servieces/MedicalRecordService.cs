using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Servieces
{
    public class MedicalRecordService
    {
        private MedicalRecordRepository medicalRecordRepository;

        public MedicalRecordService()
        {
            this.medicalRecordRepository = Singleton.Instance.MedicalRecordRepository;
        }

        public MedicalRecord GeMedicalRecord(int medicalRecordId)
        {
            return medicalRecordRepository.GetMedicalRecord(medicalRecordId);
        }

        public void WriteAll(List<MedicalRecord> medicalRecords)
        {
            medicalRecordRepository.WriteAll(medicalRecords);
        }
    }
}
