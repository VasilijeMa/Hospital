using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Servieces
{
    
    public class SpecializationReferralService
    {
        private ExaminationService examinationService;
        private PatientService patientService;
        private ScheduleService scheduleService;

        public SpecializationReferralService()
        {
            this.examinationService = new ExaminationService();
            this.patientService = new PatientService();
            this.scheduleService = new ScheduleService();
        }

        public List<SpecializationReferral> GetSpecializationReferrals(String patientUsername) {
            List<SpecializationReferral> referrals = new List<SpecializationReferral>();
            Patient patient = patientService.GetByUsername(patientUsername);
            foreach (Appointment appointment in scheduleService.GetAppointmentsForPatient(patient.Id))
            {
                if (appointment.ExaminationId != 0)
                {
                    Examination patientsExamination = examinationService.GetExaminationById(appointment.ExaminationId);
                    if (patientsExamination != null && patientsExamination.SpecializationRefferal.IsUsed == false)
                    {
                        referrals.Add(patientsExamination.SpecializationRefferal);
                    }
                }
            }
            return referrals;
        }

    }
}
