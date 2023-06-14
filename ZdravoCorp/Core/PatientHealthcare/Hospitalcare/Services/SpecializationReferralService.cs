using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services
{
    public class SpecializationReferralService
    {
        private ExaminationService examinationService;
        private PatientService patientService;
        private ScheduleService scheduleService;

        public SpecializationReferralService()
        {
            examinationService = new ExaminationService();
            patientService = new PatientService();
            scheduleService = new ScheduleService();
        }

        public List<SpecializationReferral> GetSpecializationReferrals(string patientUsername)
        {
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
