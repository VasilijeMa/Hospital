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
        public string RoomId { get; set; }
        public DateOnly StartDate{ get; set; }
        public int Duration { get; set; }

        public string AdditionalTesting { get; set; }

        public Prescription InitialTherapy { get; set; }

        public HospitalizationReferral() { }

        public HospitalizationReferral(int duration, Prescription initialTherapy, string additionalTesting, string roomId,DateOnly startDate)
        {
            Duration = duration;
            InitialTherapy = initialTherapy;
            AdditionalTesting = additionalTesting;
            RoomId = roomId;
            StartDate = startDate;
        }
    }
}
