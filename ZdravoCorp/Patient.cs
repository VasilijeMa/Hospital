using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Patient
    {
        private MedicalRecord MedicalRecord { get; set; }
        public Patient(MedicalRecord medicalRecord)
        {
            MedicalRecord = medicalRecord;
        }

    }
}
