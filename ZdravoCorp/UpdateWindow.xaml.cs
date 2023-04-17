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
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        const int APPOINTMENT_DURATION = 15;
        Singleton singleton;
        Appointment appointment;
        Patient patient;
        public UpdateWindow(Appointment appointment, Patient patient)
        {
            InitializeComponent();
            this.appointment = appointment;
            this.patient = patient;
            singleton = Singleton.Instance;
            tbId.Text = appointment.Id.ToString();
            dpDate.SelectedDate = appointment.TimeSlot.start.Date;
            dpDate.DisplayDateStart = DateTime.Now;
            tbTime.Text = appointment.TimeSlot.start.ToString("HH:mm");
            cmbDoctors.ItemsSource = singleton.doctors;
            cmbDoctors.ItemTemplate = (DataTemplate)FindResource("doctorTemplate");
            cmbDoctors.SelectedValuePath = "Id";
            cmbDoctors.SelectedValue = appointment.DoctorId;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to update the appointment?", "Congfirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            if (!inputValidation())
            {
                return;
            }
            TimeSlot timeSlot = MakeTimeSlot();
            if (timeSlot.start <= DateTime.Now)
            {
                MessageBox.Show("The selected date cannot be in the past.");
                return;
            }
            if (!singleton.Schedule.IsAvailable(timeSlot, (Doctor)cmbDoctors.SelectedItem, appointment.Id))
            {
                MessageBox.Show("Doctor is not available at choosen date and time.");
                return;
            }
            singleton.Schedule.UpdateAppointment(appointment.Id, timeSlot, (int)cmbDoctors.SelectedValue);
            singleton.Log.UpdateCancelElement(appointment, patient);
            MessageBox.Show("Appointment successfully updated.");
            this.Close();
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
                MessageBox.Show("Please enter a valid time value in \"hh:mm\" format.");
                return false;
            }
            return true;
        }
    }
}
