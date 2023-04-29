using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class AppointmentRequest
    {
        public int DoctorId { get; set; }
        public TimeOnly EarliestHour { get; set; }
        public TimeOnly LatestHour { get; set; }
        public DateTime LatestDate { get; set; }
        public Priority Priority { get; set; }
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
