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
        private PatientRepository patientRepository;

        public MedicalRecordService()
        {
            patientRepository = Singleton.Instance.PatientRepository;
            medicalRecordRepository = Singleton.Instance.MedicalRecordRepository;
        }

        public MedicalRecord GetMedicalRecordById(int medicalRecordId)
        {
            return medicalRecordRepository.GetMedicalRecord(medicalRecordId);
        }

        public void WriteAll()
        {
            medicalRecordRepository.WriteAll();
        }

        public MedicalRecord GetPatientMedicalRecord(int patientId)
        {
            int medicalRecordId = patientRepository.GetMedicalRecordId(patientId);
            return GetMedicalRecordById(medicalRecordId);
        }

        public bool IsAllergic(int patientid, Medicament medicament)
        {
            MedicalRecord medicalRecord = GetPatientMedicalRecord(patientid);
            List<string> allergies = medicalRecord.Allergens;
            return allergies.Any(medicament.Allergens.Contains);
        }
    }
}
