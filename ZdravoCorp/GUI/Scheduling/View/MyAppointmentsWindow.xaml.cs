using System.Windows;
using ZdravoCorp.Core.PatientSatisfaction.Services;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.GUI.Scheduling.ViewModel;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MyAppointmentsWindow.xaml
    /// </summary>
    public partial class MyAppointmentsWindow : Window
    {
        private ScheduleService scheduleService = new ScheduleService();
        public MyAppointmentsWindow(Patient patient, DoctorSurveyService doctorSurveyService)
        {
            InitializeComponent();
            DataContext = new MyAppointmentsViewModel(patient, this, doctorSurveyService);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            scheduleService.WriteAllAppointmens();
        }

    }
}
