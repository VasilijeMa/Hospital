using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Domain.Enums;

namespace ZdravoCorp.Domain
{
    public class SpecializationReferral
    {
        public Specialization? Specialization { get; set; }
        public int DoctorId { get; set; }

        public SpecializationReferral(Specialization? specialization, int doctorId)
        {
            DoctorId = doctorId;
            Specialization = specialization;
        }
    }
}
