using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class Prescription
    {
        public Medicament Medicament { get; set; }
        public Instruction Instruction { get; set; }

        public Prescription() { }

        public Prescription(Medicament medicament, Instruction instruction)
        {
            Medicament = medicament;
            Instruction = instruction;
        }
    }
}
