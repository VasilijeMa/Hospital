using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Servieces;
using MessageBox = System.Windows.MessageBox;

namespace ZdravoCorp
{
    public partial class ViewAppointment : Window
    {
        private List<Appointment> appointments;
        private Doctor doctor;
        private int days;
        private ScheduleService scheduleService = new ScheduleService();
        private PatientService patientService = new PatientService();

        public ViewAppointment(List<Appointment> appointments, Doctor doctor, int days)
        {
            InitializeComponent();
            this.doctor = doctor;
            this.appointments = appointments;
            this.days = days;
            DataGridLoadAppointments();
        }

        private void DataGridLoadAppointments()
        {
            DataTable dt = AddColumns();
            foreach (Appointment appointment in this.appointments)
            {
                dt.Rows.Add(appointment.Id,
                            appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"),
                            appointment.TimeSlot.start.TimeOfDay.ToString(),
                            appointment.TimeSlot.duration,
                            appointment.PatientId,
                            appointment.IdRoom,
                            appointment.IsCanceled);
            }
            this.dataGrid.ItemsSource = dt.DefaultView;
        }

        private static DataTable AddColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AppointmentID", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Duration", typeof(int));
            dt.Columns.Add("PatientID", typeof(int));
            dt.Columns.Add("RoomID", typeof(string));
            dt.Columns.Add("IsCanceled", typeof(bool));
            return dt;
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
            this.appointments = scheduleService.GetAllAppointmentsForDoctor(startDate, endTime, doctor.Id);
            DataGridLoadAppointments();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            Appointment appointment = GetSelectedAppointment(item);

            if (appointment.IsCanceled)
            {
                MessageBox.Show("Appointment is canceled.");
                return;
            }

            MakeAppointmentDoctor window = new MakeAppointmentDoctor(doctor, true, appointment.Id);
            window.tbTime.Text = appointment.TimeSlot.start.ToString("HH:mm");
            window.dpDate.SelectedDate = appointment.TimeSlot.start.Date;
            window.tbDuration.Text = appointment.TimeSlot.duration.ToString();
            window.cmbPatients.ItemsSource = patientService.GetPatients();
            window.cmbPatients.ItemTemplate = (DataTemplate)FindResource("patientTemplate");
            window.cmbPatients.SelectedValuePath = "Id";
            window.cmbPatients.SelectedValue = appointment.PatientId;
            window.ShowDialog();

            DataGridLoadAppointments();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            Appointment appointment = GetSelectedAppointment(item);

            if (appointment.IsCanceled)
            {
                MessageBox.Show("The appointment has already been cancelled.");
                return;
            }
            if (appointment.TimeSlot.start <= DateTime.Now.AddDays(1))
            {
                MessageBox.Show("The selected appointment cannot be changed.");
                return;
            }
            if (MessageBox.Show("Are you sure you want to cancel the appointment? ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                appointment.IsCanceled = true;
                scheduleService.CancelAppointment(appointment.Id);
                MessageBox.Show("Appointment successfully cancelled!");
                DataGridLoadAppointments();
            }
        }

        private void medicalRecord_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            Appointment appointment = GetSelectedAppointment(item);
            Patient patient = patientService.GetById(appointment.PatientId);
            CreateMedicalRecordWindow medicalRecord = new CreateMedicalRecordWindow(false, patient, true, null);
            medicalRecord.ShowDialog();
        }

        private Appointment GetSelectedAppointment(DataRowView item)
        {
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return null;
            }
            Appointment appointment = scheduleService.GetAppointmentById((int)item.Row["AppointmentID"]);

            return appointment;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            scheduleService.WriteAllAppointmens();
        }
    }
}
