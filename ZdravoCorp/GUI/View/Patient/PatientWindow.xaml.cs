using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.View.Patient;
using ZdravoCorp.View;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private Patient patient;
        Singleton singleton = Singleton.Instance;
        private NotificationService notificationService;
        public PatientWindow(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            lblWelcome.Content = "Welcome, " + patient.FirstName + " " + patient.LastName;
            singleton.LogRepository.SetLog(new Log());
            LogService logService = new LogService();
            logService.Count(patient.Id);
            notificationService = new NotificationService(patient.Id);
            notificationService.Start();
        }

        private void miMake_Click(object sender, RoutedEventArgs e)
        {
            MakeAppointmentWindow makeAppointmentWindow = new MakeAppointmentWindow(patient);
            makeAppointmentWindow.ShowDialog();
            if (patient.IsBlocked)
            {
                MessageBox.Show("Your account is blocked.");
                this.Close();
            }
        }

        private void MyAppointments_Click(object sender, RoutedEventArgs e)
        {
            MyAppointmentsWindow myAppointmentsWindow = new MyAppointmentsWindow(patient);
            myAppointmentsWindow.ShowDialog();
            if (patient.IsBlocked)
            {
                MessageBox.Show("Your account is blocked.");
                this.Close();
            }
        }
        private void miLogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RecommendingAppointment_Click(object sender, RoutedEventArgs e)
        {
            RecommendingAppointmentsForm recommendingAppointmentsForm = new RecommendingAppointmentsForm(patient);
            recommendingAppointmentsForm.ShowDialog();
            if (patient.IsBlocked)
            {
                MessageBox.Show("Your account is blocked.");
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            notificationService.Stop();
            ScheduleService scheduleService= new ScheduleService();
            scheduleService.WriteAllAppointmens();
        }

        private void MedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointmentsView patientAppointmentsView = new PatientAppointmentsView(patient);
            patientAppointmentsView.ShowDialog();
        }

        private void miDoctorsSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchDoctorWindow searchDoctorWindow = new SearchDoctorWindow(patient);
            searchDoctorWindow.ShowDialog();
        }

        private void miNotifications_Click(object sender, RoutedEventArgs e)
        {
            PatientNotificationsView patientNotificationsView = new PatientNotificationsView(patient);
            patientNotificationsView.ShowDialog();
        }
    }
}
