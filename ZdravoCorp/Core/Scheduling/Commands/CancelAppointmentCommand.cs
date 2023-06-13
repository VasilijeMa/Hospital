using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.Scheduling.ViewModel;

namespace ZdravoCorp.Core.Scheduling.Commands
{
    public class CancelAppointmentCommand : BaseCommand
    {
        private ScheduleService scheduleService = new ScheduleService();
        private LogService logService = new LogService();
        private MyAppointmentsViewModel viewModel;
        public CancelAppointmentCommand(MyAppointmentsViewModel viewModel)
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
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the appointment?", "Congfirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                scheduleService.CancelAppointment(viewModel.SelectedAppointment.Id);
                viewModel.Appointments = scheduleService.GetAppointmentsForPatient(viewModel.Patient.Id);
                logService.UpdateCancelElement(viewModel.SelectedAppointment, viewModel.Patient);
                if (viewModel.Patient.IsBlocked)
                {
                    viewModel.View.Close();
                }
            }

        }
    }
}
