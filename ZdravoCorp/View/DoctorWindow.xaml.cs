using System;
using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Controllers;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;
using ZdravoCorp.Servieces;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        private Doctor doctor { get; set; }

        private DoctorController doctorController;

        public DoctorWindow(Doctor doctor)
        {
            InitializeComponent();
            doctorController = new DoctorController();
            this.doctor = doctor;
            SetFields(doctor);
            doctorController.showNotification(doctor.Id);
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
            doctorController.MakeAppointment(doctor);
        }

        private void ViewOneDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            doctorController.ViewOneDayAppointment(doctor);
        }

        private void ViewThreeDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            doctorController.ViewThreeDayAppointment(doctor);
        }

        private void DailyScheduleClick(object sender, RoutedEventArgs e)
        {
            doctorController.DailySchedule(doctor);
        }

        private void SearchPatientClick(object sender, RoutedEventArgs e)
        {
            doctorController.SearchPatient(doctor);
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
