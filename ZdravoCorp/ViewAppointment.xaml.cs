using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace ZdravoCorp
{
    public partial class ViewAppointment : Window
    {
        private List<Appointment> appointments;
        private Doctor doctor;
        Singleton singleton;
        private int days;

        public ViewAppointment(List<Appointment> appointments, Doctor doctor, int days)
        {
            InitializeComponent();

            this.singleton = Singleton.Instance;
            this.doctor = doctor;
            this.appointments = appointments;   
            this.days = days;
            DataGridLoadAppointments();
        }

        private void DataGridLoadAppointments()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AppointmentID", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Duration", typeof(int));
            dt.Columns.Add("DoctorID", typeof(int));
            dt.Columns.Add("PatientID", typeof(int));
            dt.Columns.Add("IsCanceled", typeof(bool));
            foreach (Appointment appointment in this.appointments)
            {
                dt.Rows.Add(appointment.Id, appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"), appointment.TimeSlot.start.TimeOfDay.ToString(), appointment.TimeSlot.duration, appointment.DoctorId, appointment.PatientId, appointment.IsCanceled);
            }
            this.dataGrid.ItemsSource = dt.DefaultView;
        }

        private void changeDate_Click(object sender, RoutedEventArgs e)
        {
            if (selectDay.SelectedDate == null)
            {
                MessageBox.Show("Date is not selected.");
                return;
            }

            DateTime endTime;
            DateTime startDate = selectDay.SelectedDate.Value;
            if (days != 1)
            {
                endTime = startDate.AddDays(days);
            }
            else
            {
                endTime = startDate;
            }
            this.appointments = doctor.GetAllAppointments(startDate, endTime);
            DataGridLoadAppointments();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }

            Appointment appointment = singleton.Schedule.GetAppointment((int)item.Row["AppointmentID"]);
            
            if (appointment.IsCanceled)
            {
                MessageBox.Show("Appointment is canceled.");
                return;
            }

            MakeAppointmentDoctor window = new MakeAppointmentDoctor(doctor, true, appointment.Id);
            window.tbTime.Text = appointment.TimeSlot.start.TimeOfDay.ToString();
            window.dpDate.SelectedDate = appointment.TimeSlot.start.Date;
            window.tbDuration.Text = appointment.TimeSlot.duration.ToString();
            window.cmbPatients.ItemsSource = singleton.patients;
            window.cmbPatients.ItemTemplate = (DataTemplate)FindResource("patientTemplate");
            window.cmbPatients.SelectedValuePath = "Id";
            window.cmbPatients.SelectedValue = appointment.PatientId;
            window.ShowDialog();

            DataGridLoadAppointments();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }
            if ((bool)item.Row["IsCanceled"])
            {
                MessageBox.Show("The appointment has already been cancelled.");
                return;
            }

            Appointment appointment = singleton.Schedule.GetAppointment((int)item.Row["AppointmentID"]);

            if (appointment.TimeSlot.start <= DateTime.Now.AddDays(1))
            {
                MessageBox.Show("The selected appointment cannot be changed.");
                return;
            }
            else if (MessageBox.Show("Are you sure you want to cancel the appointment? ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                appointment.IsCanceled = true;
                singleton.Schedule.CancelAppointment(appointment.Id);
                MessageBox.Show("Appointment successfully cancelled!");
                DataGridLoadAppointments();
            }
        }

        private void medicalRecord_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }

            Appointment appointment = singleton.Schedule.GetAppointment((int)item.Row["AppointmentID"]);

            int patientId = appointment.PatientId;
            Patient selectedPatient = new Patient();

            bool check = false;
            foreach (Patient patient in singleton.patients)
            {
                if (patientId == patient.Id)
                {
                    check = true;
                    selectedPatient = patient;
                }
            }

            if (!check)
            {
                MessageBox.Show("The patient does not exist in the system");
                return;
            }

            CreateMedicalRecordWindow medicalRecord = new CreateMedicalRecordWindow(false, selectedPatient, true);
            medicalRecord.ShowDialog();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            singleton.Schedule.WriteAllAppointmens();
        }
    }
}