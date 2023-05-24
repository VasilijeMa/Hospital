using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MyAppointmentsWindow.xaml
    /// </summary>
    public partial class MyAppointmentsWindow : Window
    {
        List<Appointment> appointments;
        Singleton singleton;
        Patient patient;
        private ScheduleRepository scheduleRepository;
        public MyAppointmentsWindow(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            singleton = Singleton.Instance;
            appointments = singleton.ScheduleRepository.Schedule.Appointments;
            scheduleRepository = singleton.ScheduleRepository;
            LoadAppointmentsInDataGrid();
        }

        private void LoadAppointmentsInDataGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("DoctorID", typeof(int));
            dt.Columns.Add("IsCanceled", typeof(bool));
            foreach (Appointment appointment in appointments)
            {
                if (appointment.PatientId == patient.Id)
                {
                    dt.Rows.Add(appointment.Id, appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"), appointment.TimeSlot.start.TimeOfDay.ToString(), appointment.DoctorId, appointment.IsCanceled);
                }
            }
            dgAppointments.ItemsSource = dt.DefaultView;
        }

        private void dgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAppointments.SelectedItem == null)
            {
                return;
            }
            if (((DataRowView)dgAppointments.SelectedItem).Row["IsCanceled"].Equals(true))
            {
                btnCancel.IsEnabled = false;
            }
            else
            {
                btnCancel.IsEnabled = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dgAppointments.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }
            Appointment appointment = scheduleRepository.GetAppointmentById((int)item.Row["Id"]);
            if (appointment.TimeSlot.start <= DateTime.Now.AddDays(1))
            {
                MessageBox.Show("The selected appointment must not changed.");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the appointment?", "Congfirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ((DataRowView)dgAppointments.SelectedItem).Row["IsCanceled"] = true;
                btnCancel.IsEnabled = false;
                scheduleRepository.CancelAppointment(appointment.Id);

                LogService logService = new LogService();
                logService.UpdateCancelElement(appointment, patient);
                if (patient.IsBlocked)
                {
                    this.Close();
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dgAppointments.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }
            if (item.Row["IsCanceled"].Equals(true))
            {
                MessageBox.Show("Appointment is canceled.");
                return;
            }
            int id = (int)item.Row["Id"];
            Appointment appointment = scheduleRepository.GetAppointmentById(id);
            if (appointment.TimeSlot.start <= DateTime.Now.AddDays(1))
            {
                MessageBox.Show("The selected appointment must not changed.");
                return;
            }
            UpdateWindow updateWindow = new UpdateWindow(appointment, patient);
            updateWindow.ShowDialog();
            dgAppointments.ItemsSource = null;
            LoadAppointmentsInDataGrid();
            if (patient.IsBlocked)
            {
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            scheduleRepository.WriteAllAppointmens();
        }

    }
}
