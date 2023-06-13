using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Scheduling.Services
{
    public interface DoctorAvailability
    {
        public bool IsAvailable(TimeSlot timeSlot, int doctorId, int appointmentId = -1);
    }
}
