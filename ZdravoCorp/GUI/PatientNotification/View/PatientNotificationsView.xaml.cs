using System.Windows;
using ZdravoCorp.GUI.PatientNotification.ViewModel;

namespace ZdravoCorp.GUI.View.Patient
{
    /// <summary>
    /// Interaction logic for PatientNotificationsView.xaml
    /// </summary>
    public partial class PatientNotificationsView : Window
    {
        public PatientNotificationsView(Core.Domain.Patient patient)
        {
            InitializeComponent();
            DataContext = new PatientNotificationsViewModel(patient);
        }
    }
}
