using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class SubmitNotificationCommand : BaseCommand
    {
        private NotificationFormViewModel viewModel;
        private NotificationService notificationService;
        public SubmitNotificationCommand(NotificationFormViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public override void Execute(object? parameter)
        {
            notificationService = new NotificationService(viewModel.Patient.Id);
            if (viewModel.Notification == null)
            {
                if (!viewModel.IsValid())
                {
                    MessageBox.Show("Fill in all fields with valid data");
                    return;
                }
                notificationService.CreateNotification(viewModel.Message, viewModel.Patient.Id,
                    viewModel.TimesPerDay, viewModel.MinutesBefore, viewModel.Date);
                MessageBox.Show("Successfully created notification!");
            }
            else
            {
                notificationService.UpdateNotification(viewModel.Notification.Id, viewModel.Message,
                    viewModel.TimesPerDay, viewModel.MinutesBefore);
                MessageBox.Show("Successfully updated notification!");
            }
            viewModel.View.Close();
        }
    }
}
