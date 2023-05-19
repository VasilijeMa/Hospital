using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Domain
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
            string doctorsObservation, string doctorsConclusion)
        {
            AppointmentId = appointmentId;
            PatientId = patientId;
            Symptoms = symptoms;
            DoctorsObservation = doctorsObservation;
            DoctorsConclusion = doctorsConclusion;
        }
    }
}
