using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Repositories
{
    public class LogRepository : ILogRepository
    {
        private Log log;

        public Log Log { get; set; }

        public LogRepository()
        {
            Log = new Log();
            Log.Elements = Load();
            Refresh();
            Write();
            Log.MakeCounter = 0;
            Log.UpdateCancelCounter = 0;
        }

        public List<LogElement> Load()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/log.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<LogElement>>(json);
        }

        public void Write()
        {
            string json = JsonConvert.SerializeObject(Log.Elements, Formatting.Indented);
            File.WriteAllText("./../../../data/log.json", json);
        }

        public void AddElement(Appointment appointment, Patient patient)
        {
            Log.Elements.Add(new LogElement(appointment, DateTime.Now, "make"));
            Log.MakeCounter++;
            Write();
        }

        public void UpdateCancelElement(Appointment appointment, Patient patient)
        {
            Log.Elements.Add(new LogElement(appointment, DateTime.Now, "updateCancel"));
            Log.UpdateCancelCounter++;
            Write();
        }

        public void Refresh()
        {
            for (int i = 0; i < Log.Elements.Count; i++)
            {
                if (Log.Elements[i].DateTime.AddDays(30) <= DateTime.Now)
                {
                    Log.Elements.Remove(Log.Elements[i]);
                    i--;
                }
            }
        }

        public Log GetLog()
        {
            return log;
        }

        public void SetLog(Log log)
        {
            Log = log;
        }
    }
}
