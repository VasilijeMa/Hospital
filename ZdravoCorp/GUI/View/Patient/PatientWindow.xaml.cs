using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PatientNotification.Services;
using ZdravoCorp.Core.PatientSatisfaction.Services;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.View.Patient;
using ZdravoCorp.View;

namespace ZdravoCorp
{
    public partial class PatientWindow : Window
    {
        private Patient patient;
        private NotificationService notificationService;
        private DoctorSurveyService doctorSurveyService;
        private HospitalSurveyService hospitalSurveyService;
        private AnamnesisService anamnesisService;
            public PatientWindow(Patient patient, DoctorSurveyService doctorSurveyService, HospitalSurveyService hospitalSurveyService, AnamnesisService anamnesisService)
        {
            InitializeComponent();
            this.doctorSurveyService = doctorSurveyService;
            this.hospitalSurveyService = hospitalSurveyService;
            this.patient = patient;
            lblWelcome.Content = "Welcome, " + patient.FirstName + " " + patient.LastName;
            LogService logService = new LogService();
            logService.SetLog(new Log());
            logService.Count(patient.Id);
            notificationService = new NotificationService(patient.Id);
            notificationService.Start();
            this.anamnesisService = anamnesisService;
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
            MyAppointmentsWindow myAppointmentsWindow = new MyAppointmentsWindow(patient, doctorSurveyService);
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
            PatientAppointmentsView patientAppointmentsView = new PatientAppointmentsView(patient, anamnesisService);
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

        private void miHospitalSurvey_Click(object sender, RoutedEventArgs e)
        {
            HospitalSurveyView hospitalSurveyView = new HospitalSurveyView(patient, hospitalSurveyService);
            hospitalSurveyView.ShowDialog();
        }
    }
}
