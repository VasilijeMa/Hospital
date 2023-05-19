using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
