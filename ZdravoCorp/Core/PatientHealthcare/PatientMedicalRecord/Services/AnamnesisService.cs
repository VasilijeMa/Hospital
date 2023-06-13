using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Model;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Repositories.Interfaces;
using ZdravoCorp.Core.Scheduling.Model;

namespace ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Services
{
    public class AnamnesisService
    {
        private IAnamnesisRepository _anamnesisRepository;

        public AnamnesisService(IAnamnesisRepository anamnesisRepository)
        {
            _anamnesisRepository = anamnesisRepository;
        }

        public List<Anamnesis> GetAnamneses()
        {
            return _anamnesisRepository.GetAnamneses();
        }

        public List<Anamnesis> GetAnamnesesContainingSubstring(string keyword)
        {
            return _anamnesisRepository.GetAnamnesesContainingSubstring(keyword);
        }

        public void WriteAll()
        {
            _anamnesisRepository.WriteAll();
        }

        public void AddAnamnesis(Anamnesis anamnesis)
        {
            _anamnesisRepository.AddAnamnesis(anamnesis);
        }

        public Anamnesis findAnamnesisById(Appointment selectedAppointment)
        {
            return _anamnesisRepository.findAnamnesisById(selectedAppointment);
        }
    }
}
