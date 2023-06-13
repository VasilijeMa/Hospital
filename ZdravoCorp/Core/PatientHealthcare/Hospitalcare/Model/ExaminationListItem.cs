using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Enums;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model
{
    public class ExaminationListItem
    {
        readonly Examination _examination;
        public int ExaminationId => _examination.Id;
        public string AdditionalTesting => _examination.HospitalizationRefferal.AdditionalTesting;
        public int MedicalRecordId => _examination.HospitalizationRefferal.Duration;
        public string Medicament => _examination.HospitalizationRefferal.InitialTherapy.Medicament.Name;
        public TimeForMedicament? TimeForMedicament => _examination.HospitalizationRefferal.InitialTherapy.Instruction.TimeForMedicament;

        public ExaminationListItem(Examination examination)
        {
            _examination = examination;
        }

        public static implicit operator Examination(ExaminationListItem examination) => examination._examination;
        public static explicit operator ExaminationListItem(Examination d) => new ExaminationListItem(d);
    }
}
