using System.Collections.Generic;

namespace ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Model
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public List<string> EarlierIllnesses { get; set; }
        public List<string> Allergens { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }

        public MedicalRecord()
        {
            Allergens = new List<string>();
            EarlierIllnesses = new List<string>();
        }

        public MedicalRecord(int id, List<string> earlierIllnesses, List<string> allergens, double height, double weight)
        {
            Id = id;
            EarlierIllnesses = earlierIllnesses;
            Allergens = allergens;
            Height = height;
            Weight = weight;
        }
    }
}
