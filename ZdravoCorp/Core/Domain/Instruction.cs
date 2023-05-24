using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain.Enums;

namespace ZdravoCorp.Core.Domain
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
