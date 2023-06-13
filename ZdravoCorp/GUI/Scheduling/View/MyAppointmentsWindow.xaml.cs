using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.Scheduling.ViewModel;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MyAppointmentsWindow.xaml
    /// </summary>
    public partial class MyAppointmentsWindow : Window
    {
        private ScheduleService scheduleService = new ScheduleService();
        public MyAppointmentsWindow(Patient patient)
        {
            InitializeComponent();
            DataContext = new MyAppointmentsViewModel(patient, this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            scheduleService.WriteAllAppointmens();
        }

    }
}
