using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Repositories.Interfaces;

namespace ZdravoCorp.Core.PatientHealthcare.Pharmacy.Services
{
    public class MedicamentService
    {
        private IMedicamentRepository _medicamentRepository;

        public MedicamentService()
        {
            _medicamentRepository = Institution.Instance.MedicamentRepository;
        }
        public void WriteAll()
        {
            _medicamentRepository.WriteAll();
        }
        public Medicament GetByName(string name)
        {
            return _medicamentRepository.GetByName(name);
        }
        public List<Medicament> GetMedicaments()
        {
            return _medicamentRepository.GetMedicaments();
        }

        public Medicament GetMedicamentById(int id)
        {
            return _medicamentRepository.GetMedicamentById(id);
        }
        public Medicament GetByPrescription(Prescription prescription)
        {
            foreach (Medicament medicament in GetMedicaments())
            {
                if (medicament.Id == prescription.Medicament.Id)
                {
                    return medicament;
                }
            }
            return null;
        }
        public int GetRequiredQuantity(Prescription prescription)
        {
            int requiredQuantity = prescription.Instruction.TimePerDay * prescription.Instruction.Duration;
            return requiredQuantity;
        }
        public bool IsAvailableThatQuantity(Prescription prescription)
        {
            Medicament medicament = GetByPrescription(prescription);
            int requiredQuantity = GetRequiredQuantity(prescription);
            if (medicament.Quantity > requiredQuantity)
            {
                return true;
            }
            return false;
        }

        public bool IsOnTime(Prescription prescription)
        {
            DateOnly startDate = prescription.DateOfGiving;
            DateOnly endDate = startDate.AddDays(prescription.Instruction.Duration);
            if (endDate.Equals(DateOnly.FromDateTime(DateTime.Now)) || endDate.AddDays(-1).Equals(DateOnly.FromDateTime(DateTime.Now)))
            {
                return true;
            }
            return false;
        }
    }
}
