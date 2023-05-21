using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZdravoCorp.Domain.Enums;
using ZdravoCorp.Servieces;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp.Domain
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
