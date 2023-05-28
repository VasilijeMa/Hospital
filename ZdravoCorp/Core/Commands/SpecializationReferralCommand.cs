using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    internal class SpecializationReferralCommand : BaseCommand
    {
        private SpecializationReferralViewModel viewModel;
        int selectedDoctorId = -1;
        Specialization? selectedSpecialization = null;
        private ScheduleRepository scheduleRepository = Singleton.Instance.ScheduleRepository;

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
                Singleton.Instance.ExaminationRepository.Add(examination);
                viewModel.Appointment.ExaminationId = examination.Id;
                scheduleRepository.WriteAllAppointmens();
            }
            else
            {
                examination = Singleton.Instance.ExaminationRepository.GetExaminationById(viewModel.Appointment.ExaminationId);
                examination.SpecializationRefferal = specializationReferral;
                Singleton.Instance.ExaminationRepository.WriteAll();
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
            return new SpecializationReferral(selectedSpecialization, selectedDoctorId);
        }
    }
}
