﻿using System;
using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        private Doctor doctor { get; set; }
        
        private DoctorService doctorService = new DoctorService();
        private ScheduleService scheduleService = new ScheduleService();
        public DoctorWindow(Doctor doctor)
        {
            InitializeComponent();
            this.doctor = doctor;
            SetFields(doctor);
            showNotification(doctor.Id);
        }
        public void showNotification(int doctorId)
        {
            NotificationAboutCancelledAppointmentRepository notificationAppointmentRepository =
                new NotificationAboutCancelledAppointmentRepository();

            foreach (NotificationAboutCancelledAppointment notification in Singleton.Instance.NotificationAboutCancelledAppointmentRepository.Notifications)
            {
                if ((notification.DoctorId == doctorId) && (!notification.isShown))
                {
                    MessageBox.Show("Your appointment with id: " + notification.AppointmenntId.ToString() + " is cancalled.");
                    notification.isShown = true;
                    notificationAppointmentRepository.Add(notification);
                    NotificationAboutCancelledAppointmentRepository.WriteAll(notificationAppointmentRepository.Notifications);
                    return;
                }
            }
        }
        private void SetFields(Doctor doctor)
        {
            nameTxt.Text = doctor.FirstName;
            lastNameTxt.Text = doctor.LastName;
            idTxt.Text = doctor.Id.ToString();
            specializationTxt.Text = doctor.Specialization.ToString();
        }
        private void MakeAppointmentClick(object sender, RoutedEventArgs e)
        {
            MakeAppointmentDoctor appointmentDoctor = new MakeAppointmentDoctor(doctor, false);
            appointmentDoctor.Show();
        }

        private void ViewOneDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            var appointments = scheduleService.GetAllAppointmentsForDoctor(DateTime.Now.Date, DateTime.Now.Date, doctor.Id);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 1);
            appointmentDoctor.Show();
        }

        private void ViewThreeDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            DateTime endDate = DateTime.Now.AddDays(3);
            var appointments = scheduleService.GetAllAppointmentsForDoctor(DateTime.Now.Date, endDate.Date, doctor.Id);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 3);
            appointmentDoctor.Show();
        }

        private void DailyScheduleClick(object sender, RoutedEventArgs e)
        {
            var appointments = scheduleService.GetAllAppointmentsForDoctor(DateTime.Now.Date, DateTime.Now.Date, doctor.Id);
            DailyAppointmentView dailySchedule = new DailyAppointmentView(appointments, doctor);
            dailySchedule.ShowDialog();
        }

        private void SearchPatientClick(object sender, RoutedEventArgs e)
        {
            SearchPatientWindow searchPatient = new SearchPatientWindow(doctor);
            searchPatient.Show();
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ScheduleRepository scheduleRepository = Singleton.Instance.ScheduleRepository;
            scheduleRepository.WriteAllAppointmens();
        }
    }
}