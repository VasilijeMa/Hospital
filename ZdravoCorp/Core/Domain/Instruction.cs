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
        public int TimePerDay { get; set; }
        public TimeForMedicament? TimeForMedicament { get; set; }

        public Instruction() { }

        public Instruction(int timePerDay, TimeForMedicament? timeForMedicament)
        {
            TimePerDay = timePerDay;
            TimeForMedicament = timeForMedicament;
        }
    }
}
