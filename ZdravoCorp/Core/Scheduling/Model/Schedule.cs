using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.Scheduling.Model
{
    public class Schedule
    {
        public List<Appointment> TodaysAppointments { get; set; }
        public List<Appointment> Appointments { get; set; }
        public Dictionary<DateTime, List<Appointment>> DailyAppointments { get; set; }

        public Schedule() { }

        public Schedule(List<Appointment> appointments)
        {
            Appointments = appointments;
        }
    }
}
