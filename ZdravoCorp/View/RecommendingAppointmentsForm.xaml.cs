﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.Enums;
using ZdravoCorp.Repositories;
using ZdravoCorp.Servieces;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for RecommendingAppointmentsForm.xaml
    /// </summary>
    public partial class RecommendingAppointmentsForm : Window
    {
        Singleton singleton;
        Patient patient;
        List<Appointment> recommendedAppointments;
        public RecommendingAppointmentsForm(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            singleton = Singleton.Instance;
            cmbDoctors.ItemsSource = singleton.doctors;
            cmbDoctors.ItemTemplate = (DataTemplate)FindResource("doctorTemplate");
            cmbDoctors.SelectedValuePath = "Id";
            dpLDate.DisplayDateStart = DateTime.Now;
        }
        private void LoadAppointmentsInDataGrid(List<Appointment> appointments)
        {
            DataTable dt = LoadColumns();
            foreach (Appointment appointment in appointments)
            {
                dt.Rows.Add(appointment.Id, appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"), appointment.TimeSlot.start.TimeOfDay.ToString(@"hh\:mm"), appointment.DoctorId, false);
            }
            dgAppointments.ItemsSource = dt.DefaultView;
        }

        private static DataTable LoadColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("DoctorID", typeof(int));
            dt.Columns.Add("IsCanceled", typeof(bool));
            return dt;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            if (!inputValidation()) return;
            AppointmentRequest appointmentRequest = CreateAppointmentRequest();
            if (appointmentRequest == null) return;
            ScheduleService scheduleService = new ScheduleService();
            recommendedAppointments = scheduleService.GetAppointmentsByRequest(appointmentRequest, patient.Id);
            dgAppointments.ItemsSource = null;
            LoadAppointmentsInDataGrid(recommendedAppointments);
            if (recommendedAppointments.Count() == 1)
            {
                ScheduleRepository scheduleRepository = new ScheduleRepository();
                scheduleRepository.CreateAppointment(recommendedAppointments[0]);
                LogRepository.AddElement(recommendedAppointments[0], patient);
                MessageBox.Show("Appointment successfully created.");
                this.Close();
            }
        }

        private AppointmentRequest CreateAppointmentRequest()
        {
            DateTime date = dpLDate.SelectedDate.Value;
            if (date.Date < DateTime.Now.Date)
            {
                MessageBox.Show("The selected date cannot be in the past.");
                return null;
            }
            Priority priority = GetPriority();
            TimeOnly earliestTime = new TimeOnly(int.Parse(tbETime.Text.Split(":")[0]), int.Parse(tbETime.Text.Split(":")[1]));
            TimeOnly latestTime = new TimeOnly(int.Parse(tbLTime.Text.Split(":")[0]), int.Parse(tbLTime.Text.Split(":")[1]));
            return new AppointmentRequest((Doctor)cmbDoctors.SelectedItem, earliestTime, latestTime, date, priority); ;
        }

        private Priority GetPriority()
        {
            if (rbDoctor.IsChecked == true) return Priority.Doctor;
            return Priority.TimeSlot;
        }

        public bool inputValidation()
        {
            if (tbETime.Text == "" || tbLTime.Text == "" || dpLDate.SelectedDate == null || cmbDoctors.SelectedItem == null || (rbDoctor.IsChecked == false && rbTimeSlot.IsChecked == false))
            {
                MessageBox.Show("Fill in all the fields");
                return false;
            }
            string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbLTime.Text, pattern) || !System.Text.RegularExpressions.Regex.IsMatch(tbETime.Text, pattern))
            {
                MessageBox.Show("Please enter a valid time value in \"HH:mm\" format.");
                return false;
            }
            return true;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (dgAppointments.SelectedItem == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }
            Appointment appointment = GetAppointmentFromSelectedRow();
            ScheduleRepository scheduleRepository = new ScheduleRepository();
            scheduleRepository.CreateAppointment(appointment);
            LogRepository.AddElement(appointment, patient);
            MessageBox.Show("Appointment successfully created.");
            this.Close();
            return;
        }

        private Appointment GetAppointmentFromSelectedRow()
        {
            int appointmentId = (int)((DataRowView)dgAppointments.SelectedItem).Row["Id"];
            string date = (string)((DataRowView)dgAppointments.SelectedItem).Row["Date"];
            string time = (string)((DataRowView)dgAppointments.SelectedItem).Row["Time"];
            DateTime dateTime = new DateTime(int.Parse(date.Split("-")[0]), int.Parse(date.Split("-")[1]),
                int.Parse(date.Split("-")[2]), int.Parse(time.Split(":")[0]), int.Parse(time.Split(":")[1]), 0);
            TimeSlot appointmentTimeSlot = new TimeSlot(dateTime, 15);
            int doctorId = (int)((DataRowView)dgAppointments.SelectedItem).Row["DoctorID"];
            string roomId = AppointmentService.TakeRoom(appointmentTimeSlot);
            return new Appointment(appointmentId, appointmentTimeSlot, doctorId, patient.Id, roomId);
        }

        private void dgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAppointments.SelectedItem == null)
            {
                btnSubmit.IsEnabled = false;
                return;
            }
            btnSubmit.IsEnabled = true;

        }
    }
}
