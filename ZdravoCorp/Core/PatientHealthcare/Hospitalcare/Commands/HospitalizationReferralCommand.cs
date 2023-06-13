using System;
using System.Windows;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Services;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Commands
{
    internal class HospitalizationReferralCommand : BaseCommand
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
                prescription, viewModel.Testing, "", DateOnly.FromDateTime(DateTime.Now));
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
            Instruction instruction = new Instruction(viewModel.PerDay, viewModel.SelectedTime, viewModel.DurationForMedicament);
            Prescription prescription = new Prescription(viewModel.SelectedMedicament, instruction, DateOnly.FromDateTime(DateTime.Now), false);
            return prescription;
        }
    }
}
