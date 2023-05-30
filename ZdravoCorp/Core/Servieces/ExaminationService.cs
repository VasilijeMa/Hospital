using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp.Core.Servieces
{
    public class ExaminationService
    {
        private IExaminationRepository examinationRepository;
        private ScheduleService scheduleService;
        private PatientService patientService;
        public ExaminationService()
        {
            this.examinationRepository = Singleton.Instance.ExaminationRepository;
            this.scheduleService = new ScheduleService();
            this.patientService = new PatientService();
        }

        public void WriteAll()
        {
            examinationRepository.WriteAll();
        }

        public void Add(Examination examination)
        {
            examinationRepository.Add(examination);
        }

        public Examination GetExaminationById(int examinationId)
        {
            return examinationRepository.GetExaminationById(examinationId);
        }

        public List<int> GetExaminationsIdsByPatient(String patientUsername) 
        {
            List<int> examinationsIds = new List<int>();
            MessageBox.Show(patientUsername, "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
            Patient patient = patientService.GetByUsername(patientUsername);

            MessageBox.Show(patient.ToString(), "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
            foreach (Appointment appointment in scheduleService.GetAppointmentsForPatient(patient.Id))
            {
                if (appointment.ExaminationId != 0)
                {
                    Examination patientsExamination = GetExaminationById(appointment.ExaminationId);
                    if (patientsExamination != null && patientsExamination.SpecializationRefferal.IsUsed == false)
                    {
                        examinationsIds.Add(patientsExamination.Id);
                    }
                }
            }
            return examinationsIds;
        }
    }
}
