using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.View.Patient;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class AddNotificationCommand : BaseCommand
    {
        private PatientNotificationsViewModel viewModel;
        private NotificationService notificationService;
        public AddNotificationCommand(PatientNotificationsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            NotificationFormView notificationFormView = new NotificationFormView(viewModel.Patient);
            notificationFormView.ShowDialog();
            notificationService = new NotificationService(viewModel.Patient.Id);
            viewModel.Notifications =
                new ObservableCollection<Notification>(notificationService.GetPatientNotifications());
        }
    }
}
