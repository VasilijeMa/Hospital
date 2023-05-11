using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class Anamnesis
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string Symptoms { get; set; }
        public string DoctorsObservation { get; set; }

        public string DoctorsConclusion { get; set; }
        public Anamnesis() { }
        public Anamnesis(int appointmentId, int patientId, string symptoms,
            string doctorsObservation,string doctorsConclusion )
        {
            this.AppointmentId = appointmentId;
            this.PatientId = patientId;
            this.Symptoms = symptoms;
            this.DoctorsObservation = doctorsObservation;
            this.DoctorsConclusion = doctorsConclusion;
        }

        public static List<Anamnesis> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/anamneses.json");
            var json = reader.ReadToEnd();
            List<Anamnesis> anamnesis = JsonConvert.DeserializeObject<List<Anamnesis>>(json);
            return anamnesis;
        }
        public void WriteAll(List<Anamnesis> collectionOfAnamnesis)
        {
            string json = JsonConvert.SerializeObject(collectionOfAnamnesis, Formatting.Indented);
            File.WriteAllText("./../../../data/anamneses.json", json);
        }
    }
}
