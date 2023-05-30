using ZdravoCorp.Core.Domain.Enums;
namespace ZdravoCorp.Core.Domain
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
