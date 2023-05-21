using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class NurseRepository
    {
        public static List<Nurse> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/nurse.json");
            var json = reader.ReadToEnd();
            List<Nurse> nurses = JsonConvert.DeserializeObject<List<Nurse>>(json);
            return nurses;
        }
    }
}
