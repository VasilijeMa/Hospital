using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Anamnesis
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int AppointmentId { get; set; }
        public string Observation { get; set; }
        public Anamnesis() { }
        public Anamnesis(int id, int appointmentId, DateTime dateTime, string observation)
        {
            Id = id;
            AppointmentId = appointmentId;
            DateTime = dateTime;
            Observation = observation;
        }
    }
}
