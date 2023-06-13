using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.Commands
{
    internal class ShowHospitalizationReferralCommand : BaseCommand
    {
        private HospitalizedPatientViewModel viewModel;
        private ScheduleService scheduleService = new ScheduleService();

        public ShowHospitalizationReferralCommand(HospitalizedPatientViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            Appointment appointment = scheduleService.GetAppointmentByExaminationId(viewModel.SelectedExamination.ExaminationId);
            HospitalizationReferralView hospitalizationReferralView = new HospitalizationReferralView(appointment);
            hospitalizationReferralView.additionalTesting.IsReadOnly = true;
            hospitalizationReferralView.ShowDialog();
        }
    }
}
