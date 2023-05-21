using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class AnamnesisRepository
    {
        public static List<Anamnesis> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/anamneses.json");
            var json = reader.ReadToEnd();
            List<Anamnesis> anamnesis = JsonConvert.DeserializeObject<List<Anamnesis>>(json);
            return anamnesis;
        }
        public static void WriteAll(List<Anamnesis> collectionOfAnamnesis)
        {
            string json = JsonConvert.SerializeObject(collectionOfAnamnesis, Formatting.Indented);
            File.WriteAllText("./../../../data/anamneses.json", json);
        }
    }
}
