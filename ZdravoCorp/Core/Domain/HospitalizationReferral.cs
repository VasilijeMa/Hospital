using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model;

namespace ZdravoCorp.Core.Domain
{
    public class HospitalizationReferral
    {
        public DateTime StartDate { get; set; }

        public int Duration { get; set; }

        public string AdditionalTesting { get; set; }

        public Prescription InitialTherapy { get; set; }

        public HospitalizationReferral() { }

        public HospitalizationReferral(DateTime startDate, int duration, Prescription initialTherapy, string additionalTesting)
        {
            StartDate = startDate;
            Duration = duration;
            InitialTherapy = initialTherapy;
            AdditionalTesting = additionalTesting;
        }
    }
}
