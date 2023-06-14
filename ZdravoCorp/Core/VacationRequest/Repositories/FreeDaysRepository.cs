using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Repositories.Interfaces;

namespace ZdravoCorp.Core.VacationRequest.Repositories
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
            using StreamReader reader = new("./../../../../ZdravoCorp/data/freeDays.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<FreeDays>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(freeDays, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/freeDays.json", json);
        }

        public void AddFreeDays(FreeDays free)
        {
            freeDays.Add(free);
            WriteAll();
        }

        public List<FreeDays> GetFreeDaysForDoctor(int doctorId)
        {
            List<FreeDays> freeDaysList = new List<FreeDays>();
            foreach (FreeDays freeDays in freeDays)
            {
                if (!(freeDays.DoctorId == doctorId)) continue;
                DateTime endFreeDays = freeDays.StartDate.AddDays(freeDays.Duration);
                if (endFreeDays < DateTime.Now) continue;
                freeDaysList.Add(freeDays);
            }
            return freeDaysList;
        }

        public List<FreeDays> GetAll()
        {
            List<FreeDays> requests = new List<FreeDays>();
            foreach(FreeDays request in freeDays)
            {
                requests.Add(request);
            }
            return requests;
        }

        public void SaveAll(List<FreeDays> remainingRequests)
        {
            freeDays = remainingRequests;
            WriteAll();
        }
    }
}
