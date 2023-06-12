using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Services;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class MedicamentsToAddService
    {
        private IMedicamentToAddRepository _medicamentToAddRepository;
        private MedicamentService medicamentService = new MedicamentService();
        private ExaminationService examinationService = new ExaminationService();
        public MedicamentsToAddService()
        {
            _medicamentToAddRepository = Singleton.Instance.MedicamentToAddRepository;
        }
        public void WriteAll()
        {
            _medicamentToAddRepository.WriteAll();
        }
        public List<MedicamentToAdd> GetMedicaments()
        {
            return _medicamentToAddRepository.GetMedicamentsToAdd();
        }

        public MedicamentToAdd GetMedicamenToAddtById(int id)
        {
            return _medicamentToAddRepository.GetMedicamenToAddtById(id);
        }

        public void checkOrderedMedicaments() 
        {
            foreach(MedicamentToAdd medicamentToAdd in GetMedicaments()) 
            {
                if (DateOnly.FromDateTime(DateTime.Now).Equals(medicamentToAdd.DateOfOrder.AddDays(1))) 
                {
                    if (medicamentToAdd.IsDone == false) 
                    {
                        Medicament medicament = medicamentService.GetMedicamentById(medicamentToAdd.MedicamentId);
                        List<Examination> examinations = examinationService.GetExaminationsByMedicamentId(medicament.Id);
                        foreach (Examination examination in examinations) 
                        {
                            examination.Prescription.Medicament.Quantity = medicamentToAdd.NewQuantity;
                        }
                        medicament.Quantity = medicamentToAdd.NewQuantity;
                        medicamentToAdd.IsDone = true;
                        medicamentService.WriteAll();
                        examinationService.WriteAll();
                        this.WriteAll();

                    }
                }
            }
        }
    }
}
