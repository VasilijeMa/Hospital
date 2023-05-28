using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.View.Patient;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class AddNotificationCommand : BaseCommand
    {
        private PatientNotificationsViewModel viewModel;
        public AddNotificationCommand(PatientNotificationsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            NotificationFormView notificationFormView = new NotificationFormView(viewModel.Patient);
            notificationFormView.ShowDialog();
            viewModel.Notifications =
                new ObservableCollection<Notification>(Singleton.Instance.NotificationRepository.Notifications);
        }
    }
}
