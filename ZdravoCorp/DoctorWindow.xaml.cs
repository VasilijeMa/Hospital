using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
        }

        private void MakeAppointmentClick(object sender, RoutedEventArgs e)
        {
            MakeAppointmentDoctor appointmentDoctor = new MakeAppointmentDoctor(doctor, false);
            appointmentDoctor.Show();
        }

        private void ViewOneDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            this.appointments = doctor.GetAllAppointments(DateTime.Now, DateTime.Now);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 1);
            appointmentDoctor.Show();
        }

        private void ViewThreeDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            DateTime endDate = DateTime.Now.AddDays(3);
            this.appointments = doctor.GetAllAppointments(DateTime.Now, endDate);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 3);
            appointmentDoctor.Show();
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
