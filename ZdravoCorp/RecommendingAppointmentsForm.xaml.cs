using Microsoft.VisualBasic;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for RecommendingAppointmentsForm.xaml
    /// </summary>
    public partial class RecommendingAppointmentsForm : Window
    {
        Singleton singleton;
        Patient patient;
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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            if (!inputValidation())
            {
                return;
            }
            DateTime date = dpLDate.SelectedDate.Value;
            if (date < DateTime.Now)
            {
                MessageBox.Show("The selected date cannot be in the past.");
                return;
            }
            Priority priority;
            if (rbDoctor.IsChecked == true)
            {
                priority = Priority.Doctor;
            }
            else
            {
                priority = Priority.TimeSlot;
            }
            TimeOnly earliestTime = new TimeOnly(int.Parse(tbEHour.Text.Split(":")[0]), int.Parse(tbEHour.Text.Split(":")[1]));
            TimeOnly latestTime = new TimeOnly(int.Parse(tbLHour.Text.Split(":")[0]), int.Parse(tbLHour.Text.Split(":")[1]));
            AppointmentRequest appointmentRequest = new AppointmentRequest((int)cmbDoctors.SelectedValue, earliestTime, latestTime, date, priority);
            singleton.Schedule.GetClosestTimeSlots(appointmentRequest);
        }

        public bool inputValidation()
        {
            if (tbEHour.Text == "" || tbLHour.Text == "" || dpLDate.SelectedDate == null || cmbDoctors.SelectedItem == null || (rbDoctor.IsChecked == false && rbTimeSlot.IsChecked == false))
            {
                MessageBox.Show("Fill in all the fields");
                return false;
            }
            string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbLHour.Text, pattern) || !System.Text.RegularExpressions.Regex.IsMatch(tbEHour.Text, pattern))
            {
                MessageBox.Show("Please enter a valid time value in \"HH:mm\" format.");
                return false;
            }
            return true;
        }
    }
}
