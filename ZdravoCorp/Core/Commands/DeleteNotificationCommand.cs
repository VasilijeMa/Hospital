using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class DeleteNotificationCommand : BaseCommand
    {
        private PatientNotificationsViewModel viewModel;
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
            Singleton.Instance.NotificationRepository.DeleteNotification(viewModel.Notification);
            viewModel.Notifications =
                new ObservableCollection<Notification>(Singleton.Instance.NotificationRepository.Notifications);
            MessageBox.Show("Successfully deleted notification!");
        }
    }
}
