using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;

namespace ZdravoCorp.Core.Servieces
{
    public class MedicalRecordService
    {
        private MedicalRecordRepository medicalRecordRepository;

        public MedicalRecordService()
        {
            medicalRecordRepository = Singleton.Instance.MedicalRecordRepository;
        }

        public MedicalRecord GetMedicalRecord(int medicalRecordId)
        {
            return medicalRecordRepository.GetMedicalRecord(medicalRecordId);
        }

        public void WriteAll()
        {
            medicalRecordRepository.WriteAll();
        }
    }
}
