using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.GUI.View.Patient;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
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
                new DoctorSurveyView(viewModel.Patient, viewModel.SelectedAppointment.DoctorId);
            doctorSurveyView.ShowDialog();
        }
    }
}
