using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    internal class ConfigOperation
    {
        private bool updateMedicalRecord;
        private bool viewMedicalRecord;
        private bool addAnamnesisDoctor;

        public ConfigOperation(bool updateMedicalRecord, bool viewMedicalRecord, bool addAnamnesisDoctor)
        {
            this.updateMedicalRecord = updateMedicalRecord;
            this.viewMedicalRecord = viewMedicalRecord;
            this.addAnamnesisDoctor = addAnamnesisDoctor;
        }
    }
}
