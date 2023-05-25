using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class SubmitNotificationCommand : BaseCommand
    {
        private NotificationFormViewModel viewModel;
        public SubmitNotificationCommand(NotificationFormViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public override void Execute(object? parameter)
        {
            if (viewModel.Notification == null)
            {
                Singleton.Instance.NotificationRepository.CreateNotification(viewModel.Title, viewModel.Patient.Id,
                    viewModel.TimesPerDay, viewModel.MinutesBefore);
                MessageBox.Show("Successfully created notification!");
            }
            else
            {
                Singleton.Instance.NotificationRepository.UpdateNotification(viewModel.Notification.Id, viewModel.Title,
                    viewModel.TimesPerDay, viewModel.MinutesBefore);
                MessageBox.Show("Successfully updated notification!");
            }
            viewModel.View.Close();
        }
    }
}
