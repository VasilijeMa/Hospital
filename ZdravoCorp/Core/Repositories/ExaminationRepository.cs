using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories
{
    public class ExaminationRepository
    {
        List<Examination> examinations;

        public ExaminationRepository()
        {
            examinations = LoadAll();
        }

        public List<Examination> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/examination.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Examination>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(examinations, Formatting.Indented);
            File.WriteAllText("./../../../data/examination.json", json);
        }

        public int getLastId()
        {
            try
            {
                return examinations.Max(examination => examination.Id);
            }
            catch
            {
                return 0;
            }
        }

        public void Add(Examination examination)
        {
            examination.Id = getLastId() + 1;
            examinations.Add(examination);
            WriteAll();
        }
    }
}
