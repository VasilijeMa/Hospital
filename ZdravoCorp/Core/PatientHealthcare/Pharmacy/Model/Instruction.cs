using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Enums;

namespace ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model
{
    public class Instruction
    {
        public int TimePerDay { get; set; }
        public TimeForMedicament? TimeForMedicament { get; set; }

        public int Duration { get; set; }
        public Instruction() { }

        public Instruction(int timePerDay, TimeForMedicament? timeForMedicament, int duration)
        {
            TimePerDay = timePerDay;
            TimeForMedicament = timeForMedicament;
            Duration = duration;
        }
    }
}
