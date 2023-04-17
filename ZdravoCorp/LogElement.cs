using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class LogElement
    {
        public Appointment Appointment { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; }

        public LogElement(Appointment appointment, DateTime dateTime, string type)
        {
            Appointment = appointment;
            DateTime = dateTime;
            Type = type;
        }
        public LogElement() { }
    }
}
