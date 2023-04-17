using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class Log
    {
        public List<LogElement> Elements {  get; set; }
        public int MakeCounter { get; set; }
        public int UpdateCancelCounter { get; set; }
        public Log() 
        { 
            Elements = Load();
            Refresh();
            MakeCounter = 0;
            UpdateCancelCounter = 0;
        }
        public Log(List<LogElement> elements) {  Elements = elements; }
        public List<LogElement> Load()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/log.json");
            var json = reader.ReadToEnd();
            List<LogElement> elements = JsonConvert.DeserializeObject<List<LogElement>>(json);
            return elements;
        }
        public void Write()
        {
            string json = JsonConvert.SerializeObject(Elements, Formatting.Indented);
            File.WriteAllText("./../../../data/log.json", json);
        }

        public void Refresh()
        {
            foreach (var element in Elements)
            {
                if(element.DateTime.AddDays(30) <= DateTime.Now)
                {
                    Elements.Remove(element);
                }
            }
        }

        public void Count(int patientId)
        {
            foreach(var element in Elements)
            {
                if (element.Appointment.PatientId == patientId)
                {
                    if (element.Type.Equals("make"))
                    {
                        MakeCounter++;
                    }
                    else
                    {
                        UpdateCancelCounter++;
                    }
                }
            }
        }

        public void AddElement(Appointment appointment, Patient patient)
        {
            Elements.Add(new LogElement(appointment, DateTime.Now, "make"));
            MakeCounter++;
            Write();
            CheckConditions(patient);
        }

        public void UpdateCancelElement(Appointment appointment, Patient patient)
        {
            Elements.Add(new LogElement(appointment, DateTime.Now, "updateCancel"));
            UpdateCancelCounter++;
            Write();
            CheckConditions(patient);
        }

        public void CheckConditions(Patient patient)
        {
            if(MakeCounter > 8 || UpdateCancelCounter >= 5)
            {
                patient.IsBlocked = true;
            }
            else
            {
                patient.IsBlocked = false;
            }

        }
    }
}
