using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.Domain
{
    public class Schedule
    {
        public List<Appointment> TodaysAppointments { get; set; }
        public List<Appointment> Appointments { get; set; }
        public Dictionary<DateTime, List<Appointment>> DailyAppointments { get; set; }
        public Schedule() { }
        public Schedule(List<Appointment> appointments)
        {
            this.Appointments = appointments;
        }
    }
}
