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
            singleton.Log.Count(patient.Id);
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

    }
}
