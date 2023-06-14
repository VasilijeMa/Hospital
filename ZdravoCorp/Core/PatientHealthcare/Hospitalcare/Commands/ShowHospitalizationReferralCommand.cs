using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;
using ZdravoCorp.View;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Commands
{
    internal class ShowHospitalizationReferralCommand : BaseCommand
    {
        private HospitalizedPatientViewModel viewModel;
        private ScheduleService scheduleService = new ScheduleService();
        private ExaminationService examinationService = new ExaminationService();

        public ShowHospitalizationReferralCommand(HospitalizedPatientViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (!viewModel.IsValid())
            {
                MessageBox.Show("Hositalization s already over");
                return;
            }
            Appointment appointment = scheduleService.GetAppointmentByExaminationId(viewModel.SelectedExamination.ExaminationId);
            ShowWindow(appointment);
        }

        public void ShowWindow(Appointment appointment)
        {
            HospitalizationReferralView hospitalizationReferralView = new HospitalizationReferralView(appointment);
            hospitalizationReferralView.additionalTesting.IsReadOnly = true;
            hospitalizationReferralView.startDate.IsEnabled = false;
            hospitalizationReferralView.ShowDialog();
        }
    }
}
