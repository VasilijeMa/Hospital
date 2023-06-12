using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Repositories
{
    public class FreeDaysRepository : IFreeDaysRepository
    {
        private List<FreeDays> freeDays;

        public FreeDaysRepository()
        {
            freeDays = LoadAll();
        }

        public List<FreeDays> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/freeDays.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<FreeDays>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(freeDays, Formatting.Indented);
            File.WriteAllText("./../../../data/freeDays.json", json);
        }

        public void AddFreeDays(FreeDays free)
        {
            freeDays.Add(free);
            WriteAll();
        }
    }
}
