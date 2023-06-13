using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class MedicalRecordService
    {
        private IMedicalRecordRepository medicalRecordRepository;
        private IPatientRepository patientRepository;

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

        public List<MedicalRecord> GetMedicalRecords()
        {
            return medicalRecordRepository.GetMedicalRecords();
        }

        public void AddMedicalRecord(MedicalRecord medicalRecord)
        {
            medicalRecordRepository.AddMedicalRecord(medicalRecord);
        }
    }
}
