using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Repositories;
using ZdravoCorp.Core.VacationRequest.Services;

namespace ZdravoCorp.Core.Scheduling.Services
{
    public class DoctorAvailabilityByVacation : DoctorAvailability
    {
        FreeDaysRepository freeDaysRepository = new FreeDaysRepository();

        public bool IsAvailable(TimeSlot timeSlot, int doctorId, int appointmentId = -1)
        {
            foreach (FreeDays freeDays in freeDaysRepository.GetFreeDaysForDoctor(doctorId))
            {
                TimeSlot timeSlotOfFreeDays = new TimeSlot(freeDays.StartDate, freeDays.Duration * 1440);
                if (timeSlot.OverlapWith(timeSlotOfFreeDays))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
