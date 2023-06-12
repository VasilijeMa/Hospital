using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.GUI.Scheduling.ViewModel;
using ZdravoCorp.GUI.View.Patient;

namespace ZdravoCorp.Core.PatientSatisfaction.Commands
{
    public class DoctorSurveyCommand : BaseCommand
    {
        private MyAppointmentsViewModel viewModel;
        public DoctorSurveyCommand(MyAppointmentsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            if (viewModel.SelectedAppointment == null || viewModel.SelectedAppointment.IsCanceled) return false;
            if (viewModel.SelectedAppointment.TimeSlot.start >= DateTime.Now) return false;
            return true;
        }

        public override void Execute(object? parameter)
        {
            DoctorSurveyView doctorSurveyView =
                new DoctorSurveyView(viewModel.Patient, viewModel.SelectedAppointment);
            doctorSurveyView.ShowDialog();
        }
    }
}
