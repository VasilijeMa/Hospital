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
        public List<Anamnesis> GetAnamnesesContainingSubstring(string keyword)
        {
            List<Anamnesis> tempAnamneses = new List<Anamnesis>();
            foreach (Anamnesis anamnesis in Anamneses)
            {
                if (anamnesis.DoctorsObservation.ToUpper().Contains(keyword) || anamnesis.DoctorsConclusion.ToUpper().Contains(keyword))
                {
                    tempAnamneses.Add(anamnesis);
                }
            }
            return tempAnamneses;
        }
        public Anamnesis findAnamnesisById(Appointment selectedAppointment)
        {
            foreach (Anamnesis anamnesis in Anamneses)
            {
                if (anamnesis.AppointmentId == selectedAppointment.Id)
                {
                    return anamnesis;
                }
            }
            return null;
        }
    }
}
