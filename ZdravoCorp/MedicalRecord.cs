using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ZdravoCorp
{
    class MedicalRecord
    {
        private List<Anamnesis> anamneses;
        public List<Anamnesis> GetAnamneses()
        {
            return anamneses;
        }
        public void SetAnamneses(List<Anamnesis> anamneses)
        {
            this.anamneses = anamneses; 
        }
        public MedicalRecord() { }

        public MedicalRecord(List<Anamnesis> anamneses)
        {
            SetAnamneses(anamneses);
        }
    }
}
