using System;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    internal class HospitalizationReferralCommand:BaseCommand
    {
        private HospitalizationReferralViewModel viewModel;
        private ScheduleService scheduleService = new ScheduleService();
        private MedicalRecordService medicalRecordService = new MedicalRecordService();
        private ExaminationService examinationService = new ExaminationService();
        public HospitalizationReferralCommand(HospitalizationReferralViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (!viewModel.IsValid())
            {
                MessageBox.Show("You must fill in all fields.");
                return;
            }
            if (medicalRecordService.IsAllergic(viewModel.Appointment.PatientId, viewModel.SelectedMedicament))
            {
                MessageBox.Show("The patient has an allergy to the medicament");
                return;
            }
            Prescription prescription = CreatePrescription();
            HospitalizationReferral hospitalizationReferral = new HospitalizationReferral(viewModel.Duration,
                prescription, viewModel.Testing);
            CreateExamination(hospitalizationReferral);
            MessageBox.Show("You have successfully create referral!");
        }

        private void CreateExamination(HospitalizationReferral hospitalizationReferral)
        {
            Examination examination;
            if (viewModel.Appointment.ExaminationId == 0)
            {
                examination = new Examination(hospitalizationReferral);
                examinationService.Add(examination);
                viewModel.Appointment.ExaminationId = examination.Id;
                scheduleService.WriteAllAppointmens();
            }
            else
            {
                examination = examinationService.GetExaminationById(viewModel.Appointment.ExaminationId);
                examination.HospitalizationRefferal = hospitalizationReferral;
                examinationService.WriteAll();
            }
        }

        private Prescription CreatePrescription()
        {
            Instruction instruction = new Instruction(viewModel.PerDay, viewModel.SelectedTime,5);
            Prescription prescription = new Prescription(viewModel.SelectedMedicament, instruction,DateOnly.FromDateTime(DateTime.Now),false);
            return prescription;
        }
    }
}
