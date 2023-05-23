using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Domain
{
    public class Examination
    {
        public int Id { get; set; }
        public SpecializationReferral SpecializationRefferal { get; set; }
        public HospitalizationReferral HospitalizationRefferal { get; set; }
        public Prescription Prescription { get; set; }

        public Examination(SpecializationReferral specializationRefferal)
        {
            SpecializationRefferal = specializationRefferal;
        }

        public Examination(HospitalizationReferral hospitalizationRefferal)
        {
            HospitalizationRefferal = hospitalizationRefferal;
        }
        public Examination(int Id, SpecializationReferral specializationRefferal, HospitalizationReferral hospitalizationRefferal, Prescription prescription)
        {
            this.Id = Id;
            SpecializationRefferal = specializationRefferal;
            HospitalizationRefferal = hospitalizationRefferal;
            Prescription = prescription;
        }

        public Examination() { }
    }
}
