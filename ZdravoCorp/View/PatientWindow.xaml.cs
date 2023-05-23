﻿using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;
using ZdravoCorp.Servieces;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private Patient patient;
        Singleton singleton = Singleton.Instance;
        public PatientWindow(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            lblWelcome.Content = "Welcome, " + patient.FirstName + " " + patient.LastName;
            singleton.LogRepository.Log = new Log();
            LogService logService = new LogService();
            logService.Count(patient.Id);
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
            ScheduleRepository scheduleRepository = singleton.ScheduleRepository;
            scheduleRepository.WriteAllAppointmens();
        }

        private void MedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            PatientAppointmentsView patientAppointmentsView = new PatientAppointmentsView(patient);
            patientAppointmentsView.ShowDialog();
        }
    }
}
