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
    /// Interaction logic for MakeAppointmentDoctor.xaml
    /// </summary>
    public partial class MakeAppointmentDoctor : Window
    {
        Singleton singleton;
        Doctor doctor;
        const int APPOINTMENT_DURATION = 15;

        public MakeAppointmentDoctor(Doctor doctor)
        {
            this.doctor = doctor;
            singleton = Singleton.Instance;
            InitializeComponent();
            cmbPatients.ItemsSource = singleton.patients;
            cmbPatients.ItemTemplate = (DataTemplate)FindResource("patientTemplate");
            cmbPatients.SelectedValuePath = "Id";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!inputValidation())
            {
                return;
            }
            TimeSlot timeSlot = MakeTimeSlot();

            if (!singleton.Schedule.IsAvailable(timeSlot, (Patient)cmbPatients.SelectedItem))
            {
                MessageBox.Show("Patient is not available at choosen date and time.");
                return;
            }
            if (!singleton.Schedule.IsAvailable(timeSlot, doctor))
            {
                MessageBox.Show("You are not available at choosen date and time.");
                return;
            }

            singleton.Schedule.CreateAppointment(timeSlot, doctor, (Patient)cmbPatients.SelectedItem);
            singleton.Schedule.WriteAllAppointmens();
            MessageBox.Show("Appointment successfully created.");
        }

        public TimeSlot MakeTimeSlot()
        {
            int hour = int.Parse(tbTime.Text.Split(":")[0]);
            int minutes = int.Parse(tbTime.Text.Split(":")[1]);
            DateTime dtValue = dpDate.SelectedDate.Value;
            DateTime dateTime = new DateTime(dtValue.Year, dtValue.Month, dtValue.Day, hour, minutes, 0);
            return new TimeSlot(dateTime, APPOINTMENT_DURATION);
        }

        public bool inputValidation()
        {
            if (tbTime.Text == "" || dpDate.SelectedDate == null || cmbPatients.SelectedItem == null)
            {
                MessageBox.Show("Fill in all the fields");
                return false;
            }
            string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbTime.Text, pattern))
            {
                MessageBox.Show("Please enter a valid time value in \"hh:mm\" format.");
            }
            return true;
        }
    }
}
