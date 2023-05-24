using ZdravoCorp.Core.Domain.Enums;
namespace ZdravoCorp.Core.Domain
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
