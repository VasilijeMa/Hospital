using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Anamnesis
    {
        public Appointment Appointment { get; set; }

        public Anamnesis(Appointment appointment)
        {
            Appointment = appointment;
        }
    }
}
