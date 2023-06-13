using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Repositories.Interfaces;


namespace ZdravoCorp.Core.PatientHealthcare.Pharmacy.Repositories
{
    public class MedicamentRepository : IMedicamentRepository
    {
        private List<Medicament> medicaments;

        public List<Medicament> Medicaments { get => medicaments; }

        public MedicamentRepository()
        {
            medicaments = LoadAll();
        }

        public List<Medicament> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/medicament.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Medicament>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(medicaments, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/medicament.json", json);
        }

        public Medicament GetMedicamentById(int id)
        {
            foreach (var medicament in medicaments)
            {
                if (medicament.Id == id) return medicament;
            }
            return null;
        }

        public Medicament GetByName(string name)
        {
            foreach (var medicament in medicaments)
            {
                if (medicament.Name == name) return medicament;
            }
            return null;
        }
        public List<Medicament> GetMedicaments()
        {
            return Medicaments;
        }
    }
}
