using ZdravoCorp.Core.Enums;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model
{
    public class SpecializationReferral
    {
        public Specialization? Specialization { get; set; }
        public int DoctorId { get; set; }

        public bool IsUsed { get; set; }

        public SpecializationReferral(Specialization? specialization, int doctorId, bool isUsed)
        {
            DoctorId = doctorId;
            Specialization = specialization;
            IsUsed = isUsed;
        }
    }
}
