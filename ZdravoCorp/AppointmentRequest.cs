using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    internal class AppointmentRequest
    {
        int DoctorId { get; set; }
        TimeOnly EarliestHour { get; set; }
        TimeOnly LatestHour { get; set; }
        DateTime LatestDate { get; set; }
        Priority Priority { get; set; }
        public AppointmentRequest() { }

        public AppointmentRequest(int doctorId, TimeOnly earliestHour, TimeOnly latestHour, DateTime latestDate, Priority priority)
        {
            DoctorId = doctorId;
            EarliestHour = earliestHour;
            LatestHour = latestHour;
            LatestDate = latestDate;
            Priority = priority;
        }
    }
}
