using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class AnamnesisRepository
    {
        private List<Anamnesis> anamnesis;
        public List<Anamnesis> Anamneses { get => anamnesis; }
        public AnamnesisRepository()
        {
            anamnesis = LoadAll();
        }
        public List<Anamnesis> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/anamneses.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Anamnesis>>(json);
        }
        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(anamnesis, Formatting.Indented);
            File.WriteAllText("./../../../data/anamneses.json", json);
        }
    }
}
