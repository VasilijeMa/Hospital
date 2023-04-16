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
    /// Interaction logic for MakeAppointmentWindow.xaml
    /// </summary>
    public partial class MakeAppointmentWindow : Window
    {
        const int APPOINTMENT_DURATION = 15;
        Singleton singleton;
        Patient patient;
        public MakeAppointmentWindow(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            singleton = Singleton.Instance;
            cmbDoctors.ItemsSource = singleton.doctors;
            cmbDoctors.ItemTemplate = (DataTemplate)FindResource("doctorTemplate");
            cmbDoctors.SelectedValuePath = "Id";
            dpDate.DisplayDateStart = DateTime.Now;
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
            if (timeSlot.start <= DateTime.Now)
            {
                MessageBox.Show("The selected date must not be in the past.");
                return;
            }
            if (!singleton.Schedule.IsAvailable(timeSlot, (Doctor)cmbDoctors.SelectedItem))
            {
                MessageBox.Show("Doctor is not available at choosen date and time.");
                return;
            }
            Appointment appointment = singleton.Schedule.CreateAppointment(timeSlot, (Doctor)cmbDoctors.SelectedItem, patient);
            singleton.Log.AddElement(appointment, patient);
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
            if (tbTime.Text == "" || dpDate.SelectedDate == null || cmbDoctors.SelectedItem == null)
            {
                MessageBox.Show("Fill in all the fields");
                return false;
            }
            string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbTime.Text, pattern))
            {
                MessageBox.Show("Please enter a valid time value in \"HH:mm\" format.");
                return false;
            }
            return true;
        }
    }
}
