using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.View.Patient;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class UpdateNotificationCommand : BaseCommand
    {
        private PatientNotificationsViewModel viewModel;
        public UpdateNotificationCommand(PatientNotificationsViewModel viewModel)
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
            NotificationFormView notificationFormView =
                new NotificationFormView(viewModel.Patient, viewModel.Notification);
            notificationFormView.ShowDialog();
            viewModel.Notifications =
                new ObservableCollection<Notification>(Singleton.Instance.NotificationRepository.Notifications);
        }
    }
}
