using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.Domain;


namespace ZdravoCorp.Core.Repositories
{
    public class MedicamentRepository
    {
        private List<Medicament> medicaments;

        public List<Medicament> Medicaments { get => medicaments; }


        public MedicamentRepository()
        {
            medicaments = LoadAll();
        }

        private List<Medicament> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/medicament.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Medicament>>(json);
        }

        private void WriteAll()
        {
            string json = JsonConvert.SerializeObject(medicaments, Formatting.Indented);
            File.WriteAllText("./../../../data/medicament.json", json);
        }

        public Medicament GetMedicamentById(int id)
        {
            foreach (var medicament in medicaments)
            {
                if (medicament.Id == id) return medicament;
            }
            return null;
        }
    }
}
