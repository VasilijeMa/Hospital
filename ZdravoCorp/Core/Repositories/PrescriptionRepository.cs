using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories
{
    public class PrescriptionRepository
    {
        private List<Prescription> prescriptions;
        public List<Prescription> Prescriptions { get => prescriptions; }

        public PrescriptionRepository()
        {
            prescriptions = LoadAll();
        }
        public List<Prescription> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/prescriptions.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Prescription>>(json);
        }
        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(prescriptions, Formatting.Indented);
            File.WriteAllText("./../../../data/prescriptions.json", json);
        }

        //public Prescription getByAppointmentId(int appointmentId)
        //{
        //    foreach (var prescription in prescriptions)
        //    {
        //        if (prescription.AppointmentId == appointmentId) return prescription;
        //    }
        //    return null;
        //}

        public void AddPrescription(Prescription prescription)
        {
            prescriptions.Add(prescription);
        }
    }
}
