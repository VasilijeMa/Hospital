using System;
using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        private Doctor doctor { get; set; }
        private List<Appointment> appointments { get; set; }

        public DoctorWindow(Doctor doctor)
        {
            this.doctor = doctor;
            InitializeComponent();
            nameTxt.Text = doctor.FirstName;
            lastNameTxt.Text = doctor.LastName;
            idTxt.Text = doctor.Id.ToString();
            specializationTxt.Text = doctor.Specialization.ToString();
            showNotification();
        }

        private void showNotification()
        {
            foreach (NotificationAboutCancelledAppointment notification in Singleton.Instance.notificationAboutCancelledAppointment)
            {
                if ((notification.DoctorId == this.doctor.Id) && (!notification.isShown))
                {
                    MessageBox.Show("Your appointment with id: " + notification.AppointmenntId.ToString() + " is cancalled.");
                    notification.isShown = true;
                    Singleton.Instance.notificationAboutCancelledAppointment.Add(notification);
                    NotificarionAboutCancelledAppointmentRepository.WriteAll(Singleton.Instance.notificationAboutCancelledAppointment);
                    return;
                }
            }
        }
        private void MakeAppointmentClick(object sender, RoutedEventArgs e)
        {
            MakeAppointmentDoctor appointmentDoctor = new MakeAppointmentDoctor(doctor, false);
            appointmentDoctor.Show();
        }

        private void ViewOneDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            this.appointments = doctor.GetAllAppointments(DateTime.Now.Date, DateTime.Now.Date);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 1);
            appointmentDoctor.Show();
        }

        private void ViewThreeDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            DateTime endDate = DateTime.Now.AddDays(3);
            this.appointments = doctor.GetAllAppointments(DateTime.Now.Date, endDate.Date);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 3);
            appointmentDoctor.Show();
        }

        private void DailyScheduleClick(object sender, RoutedEventArgs e)
        {
            this.appointments = doctor.GetAllAppointments(DateTime.Now.Date, DateTime.Now.Date);
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
            Singleton.Instance.Schedule.WriteAllAppointmens();
        }
    }
}
