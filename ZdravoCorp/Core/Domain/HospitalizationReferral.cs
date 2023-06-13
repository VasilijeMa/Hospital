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
        public int RoomId { get; set; }

        public DateOnly StartDate{ get; set; }
        public bool IsUsed { get; set; }
        public int Duration { get; set; }

        public string AdditionalTesting { get; set; }

        public Prescription InitialTherapy { get; set; }

        public HospitalizationReferral() { }

        public HospitalizationReferral(int duration, Prescription initialTherapy, string additionalTesting, bool isUsed, int roomId,DateOnly startDate)
        {
            Duration = duration;
            InitialTherapy = initialTherapy;
            AdditionalTesting = additionalTesting;
            IsUsed = isUsed;
            RoomId = roomId;
            StartDate = startDate;
        }
    }
}
