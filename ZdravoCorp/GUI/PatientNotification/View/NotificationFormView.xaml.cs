using System;
using System.Windows;
using ZdravoCorp.Core.PatientNotification.Model;
using ZdravoCorp.GUI.PatientNotification.ViewModel;

namespace ZdravoCorp.GUI.View.Patient
{
    /// <summary>
    /// Interaction logic for NotificationFormView.xaml
    /// </summary>
    public partial class NotificationFormView : Window
    {
        public NotificationFormView(Core.Domain.Patient patient, Notification notification = null)
        {
            InitializeComponent();
            dpDate.DisplayDateStart = DateTime.Now;
            DataContext = new NotificationFormViewModel(patient, this, notification);
            if (notification != null)
            {
                tbMessage.Visibility = Visibility.Hidden;
                dpDate.Visibility = Visibility.Hidden;
                lblMessage.Visibility = Visibility.Hidden;
                lblDate.Visibility = Visibility.Hidden;
            }
            else
            {
                lblMinutesBefore.Visibility = Visibility.Hidden;
                tbMinutesBefore.Visibility = Visibility.Hidden;
            }
        }
    }
}
