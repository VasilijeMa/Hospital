using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    internal class HospitalizationReferralCommand:BaseCommand
    {
        private HospitalizationReferralViewModel viewModel;
        private ScheduleRepository scheduleRepository = Singleton.Instance.ScheduleRepository;
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
                Singleton.Instance.ExaminationRepository.Add(examination);
                viewModel.Appointment.ExaminationId = examination.Id;
                scheduleRepository.WriteAllAppointmens();
            }
            else
            {
                examination = Singleton.Instance.ExaminationRepository.GetExaminationById(viewModel.Appointment.ExaminationId);
                examination.HospitalizationRefferal = hospitalizationReferral;
                Singleton.Instance.ExaminationRepository.WriteAll();
            }
        }

        private Prescription CreatePrescription()
        {
            Instruction instruction = new Instruction(viewModel.PerDay, viewModel.SelectedTime);
            Prescription prescription = new Prescription(viewModel.SelectedMedicament, instruction);
            return prescription;
        }
    }
}
