using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZdravoCorp.Domain;
using ZdravoCorp.Servieces;

namespace ZdravoCorp.Repositories
{
    public class LogRepository
    {
        public static List<LogElement> Load()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/log.json");
            var json = reader.ReadToEnd();
            List<LogElement> elements = JsonConvert.DeserializeObject<List<LogElement>>(json);
            return elements;
        }
        public static void Write()
        {
            string json = JsonConvert.SerializeObject(Singleton.Instance.Log.Elements, Formatting.Indented);
            File.WriteAllText("./../../../data/log.json", json);
        }

        public static void AddElement(Appointment appointment, Patient patient)
        {
            Singleton.Instance.Log.Elements.Add(new LogElement(appointment, DateTime.Now, "make"));
            Singleton.Instance.Log.MakeCounter++;
            Write();
            LogService.CheckConditions(patient);
        }

        public static void UpdateCancelElement(Appointment appointment, Patient patient)
        {
            Singleton.Instance.Log.Elements.Add(new LogElement(appointment, DateTime.Now, "updateCancel"));
            Singleton.Instance.Log.UpdateCancelCounter++;
            Write();
            LogService.CheckConditions(patient);
        }
        public static void Refresh(List<LogElement> elements)
        {
            for(int i = 0; i < elements.Count;i++)
            {
                if (elements[i].DateTime.AddDays(30) <= DateTime.Now)
                {
                    elements.Remove(elements[i]);
                    i--;
                }
            }
        }
    }
}
