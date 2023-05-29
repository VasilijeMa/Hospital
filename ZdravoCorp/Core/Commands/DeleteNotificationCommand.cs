using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class DeleteNotificationCommand : BaseCommand
    {
        private PatientNotificationsViewModel viewModel;
        private NotificationService notificationService;

        public DeleteNotificationCommand(PatientNotificationsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (viewModel.Notification == null)
            {
                MessageBox.Show("Notification is not selected!");
                return;
            }

            notificationService = new NotificationService(viewModel.Patient.Id);
            notificationService.DeleteNotification(viewModel.Notification);
            viewModel.Notifications =
                new ObservableCollection<Notification>(notificationService.GetPatientNotifications());
            MessageBox.Show("Successfully deleted notification!");
            viewModel.Notification = null;
        }
    }
}
