using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.GUI.Scheduling.ViewModel;

namespace ZdravoCorp.Core.Scheduling.Commands
{
    internal class UpdateAppointmentCommand : BaseCommand
    {
        private ScheduleService scheduleService = new ScheduleService();
        private MyAppointmentsViewModel viewModel;
        public UpdateAppointmentCommand(MyAppointmentsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            if (viewModel.SelectedAppointment == null || viewModel.SelectedAppointment.IsCanceled) return false;
            if (viewModel.SelectedAppointment.TimeSlot.start <= DateTime.Now.AddDays(1)) return false;
            return true;
        }
        public override void Execute(object? parameter)
        {
            UpdateWindow updateWindow = new UpdateWindow(viewModel.SelectedAppointment, viewModel.Patient);
            updateWindow.ShowDialog();
            viewModel.Appointments.Clear();
            viewModel.Appointments = scheduleService.GetAppointmentsForPatient(viewModel.Patient.Id); ;
            if (viewModel.Patient.IsBlocked)
            {
                viewModel.View.Close();
            }
        }
    }
}
