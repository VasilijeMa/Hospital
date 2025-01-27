﻿using System.Windows;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Commands
{
    internal class SpecializationReferralCommand : BaseCommand
    {
        private SpecializationReferralViewModel viewModel;
        int selectedDoctorId = -1;
        Specialization? selectedSpecialization = null;
        private ScheduleService scheduleService = new ScheduleService();
        private ExaminationService examinationService = new ExaminationService();

        public SpecializationReferralCommand(SpecializationReferralViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            if (viewModel.IsSpecialization && viewModel.SelectedSpecialization == null)
            {
                MessageBox.Show("You must select specialization!");
                return false;
            }
            if (viewModel.IsDoctor && viewModel.SelectedDoctor == null)
            {
                MessageBox.Show("You must select doctor!");
                return false;
            }
            return true;
        }

        public override void Execute(object? parameter)
        {

            if (!viewModel.IsSpecialization && !viewModel.IsDoctor)
            {
                MessageBox.Show("You must select one option!");
                return;
            }
            SpecializationReferral specializationReferral = CreateSpecializationReferral();
            CreateExamination(specializationReferral);
            MessageBox.Show("You have successfully create referral!");
        }

        private void CreateExamination(SpecializationReferral specializationReferral)
        {
            Examination examination;
            if (viewModel.Appointment.ExaminationId == 0)
            {
                examination = new Examination(specializationReferral);
                examinationService.Add(examination);
                viewModel.Appointment.ExaminationId = examination.Id;
                scheduleService.WriteAllAppointmens();
            }
            else
            {
                examination = examinationService.GetExaminationById(viewModel.Appointment.ExaminationId);
                examination.SpecializationRefferal = specializationReferral;
                examinationService.WriteAll();
            }
        }

        private SpecializationReferral CreateSpecializationReferral()
        {
            if (viewModel.SelectedDoctor != null && viewModel.IsDoctor)
            {
                selectedDoctorId = viewModel.SelectedDoctor.Id;
                selectedSpecialization = null;
            }
            if (viewModel.IsSpecialization)
            {
                selectedSpecialization = viewModel.SelectedSpecialization;
                selectedDoctorId = -1;
            }
            return new SpecializationReferral(selectedSpecialization, selectedDoctorId, false);
        }
    }
}
