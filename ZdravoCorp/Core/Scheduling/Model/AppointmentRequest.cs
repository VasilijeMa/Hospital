using System;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.UserManager.Model;

namespace ZdravoCorp.Core.Scheduling.Model
{
    public class AppointmentRequest
    {
        public Doctor Doctor { get; set; }
        public TimeOnly EarliesTime { get; set; }
        public TimeOnly LatestTime { get; set; }
        public DateTime LatestDate { get; set; }
        public Priority Priority { get; set; }

        public AppointmentRequest() { }

        public AppointmentRequest(Doctor doctor, TimeOnly earliestHour, TimeOnly latestHour, DateTime latestDate, Priority priority)
        {
            Doctor = doctor;
            EarliesTime = earliestHour;
            LatestTime = latestHour;
            LatestDate = latestDate;
            Priority = priority;
        }
    }
}
