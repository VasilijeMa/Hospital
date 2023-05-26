using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    internal class HospitalizationReferralCommand:BaseCommand
    {
        private HospitalizationReferralViewModel viewModel;
        public HospitalizationReferralCommand(HospitalizationReferralViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            Prescription prescription = CreatePrescription();
            HospitalizationReferral hospitalizationReferral = new HospitalizationReferral(viewModel.Duration,
                prescription, viewModel.Testing);
            CreateExamination(hospitalizationReferral);
        }

        private void CreateExamination(HospitalizationReferral hospitalizationReferral)
        {
            Examination examination;
            if (viewModel.Appointment.ExaminationId == 0)
            {
                examination = new Examination(hospitalizationReferral);
                Singleton.Instance.ExaminationRepository.Add(examination);
                viewModel.Appointment.ExaminationId = examination.Id;
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
