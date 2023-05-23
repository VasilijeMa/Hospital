using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Domain.Enums;

namespace ZdravoCorp.Domain
{
    public class Instruction
    {
        private int TimePerDay { get; set; }
        private TimeForMedicament TimeForMedicament { get; set; }

        public Instruction(int timePerDay, TimeForMedicament timeForMedicament)
        {
            TimePerDay = timePerDay;
            TimeForMedicament = timeForMedicament;
        }
    }
}
