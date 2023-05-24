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
        public List<Medicament> Medicaments { get; set; }
        public Instruction Instruction { get; set; }

        public Prescription() { }

        public Prescription(List<Medicament> medicaments, Instruction instruction)
        {
            Medicaments = medicaments;
            Instruction = instruction;
        }
    }
}
