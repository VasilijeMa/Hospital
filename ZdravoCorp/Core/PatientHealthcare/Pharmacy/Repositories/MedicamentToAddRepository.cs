using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Repositories.Interfaces;

namespace ZdravoCorp.Core.PatientHealthcare.Pharmacy.Repositories
{
    public class MedicamentToAddRepository : IMedicamentToAddRepository
    {
        private List<MedicamentToAdd> medicamentsToAdd;

        public List<MedicamentToAdd> Medicaments { get => medicamentsToAdd; }

        public MedicamentToAddRepository()
        {
            medicamentsToAdd = LoadAll();
        }
        public MedicamentToAdd GetMedicamenToAddtById(int id)
        {
            foreach (var medicament in medicamentsToAdd)
            {
                if (medicament.MedicamentId == id) return medicament;
            }
            return null;
        }

        public List<MedicamentToAdd> GetMedicamentsToAdd()
        {
            return medicamentsToAdd;
        }

        public List<MedicamentToAdd> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/medicamentstoadd.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<MedicamentToAdd>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(medicamentsToAdd, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/medicamentstoadd.json", json);
        }

    }
}
