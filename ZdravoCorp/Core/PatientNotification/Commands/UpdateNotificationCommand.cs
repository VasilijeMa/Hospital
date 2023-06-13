using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using ZdravoCorp.Core.PatientNotification.Model;
using ZdravoCorp.Core.PatientNotification.Services;
using ZdravoCorp.GUI.PatientNotification.ViewModel;
using ZdravoCorp.GUI.View.Patient;

namespace ZdravoCorp.Core.PatientNotification.Commands
{
    public class UpdateNotificationCommand : BaseCommand
    {
        private PatientNotificationsViewModel viewModel;
        private NotificationService notificationService;

        public UpdateNotificationCommand(PatientNotificationsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (viewModel.Notification.Date != null)
            {
                MessageBox.Show("The notification cannot be changed!");
                return;
            }
            NotificationFormView notificationFormView =
                new NotificationFormView(viewModel.Patient, viewModel.Notification);
            notificationFormView.ShowDialog();
            notificationService = new NotificationService(viewModel.Patient.Id);
            viewModel.Notifications =
                new ObservableCollection<Notification>(notificationService.GetPatientNotifications());
            viewModel.Notification = null;
        }
    }
}
