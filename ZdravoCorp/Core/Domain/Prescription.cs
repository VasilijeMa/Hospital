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
        //public int AppointmentId { get; set; }
        public Medicament Medicament { get; set; }
        public Instruction Instruction { get; set; }

        public DateOnly DateOfGiving { get; set; }

        public bool IsUsed { get; set; }
        public Prescription() { }

        //public Prescription(int medicament, Instruction instruction)
        //{
        //    MedicamentId = medicament;
        //    Instruction = instruction;
        //}

        public Prescription(Medicament medicament, Instruction instruction, DateOnly dateOfGiving, bool isUsed)
        {
            //AppointmentId = appointmentId;
            Medicament = medicament;
            Instruction = instruction;
            DateOfGiving = dateOfGiving;
            IsUsed = isUsed;
        }
    }
}
