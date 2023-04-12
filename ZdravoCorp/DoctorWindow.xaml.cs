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

        public DoctorWindow(Doctor doctor)
        {
            this.doctor = doctor;

            InitializeComponent();

            var app = doctor.GetAllAppointments(DateTime.Parse("05/29/2023 05:50"), DateTime.Parse("05/29/2023 05:50"));

            string s = "";

            foreach (Appointment p in app)
            {
                s += p.ToString();
            }

           // tbUsername.Text = app.Count.ToString();
        }

        private void makeAppointmentClick(object sender, RoutedEventArgs e)
        {
            MakeAppointmentDoctor appointmentDoctor = new MakeAppointmentDoctor(doctor);
            appointmentDoctor.Show();
        }
    }
}
