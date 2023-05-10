using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
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
        private void LoadAppointmentsInDataGrid(List<TimeSlot> timeSlots)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("DoctorID", typeof(int));
            dt.Columns.Add("IsCanceled", typeof(bool));
            foreach (TimeSlot timeSlot in timeSlots)
            {
                int appointmentId = singleton.Schedule.getLastId();
                dt.Rows.Add(appointmentId, timeSlot.start.Date.ToString("yyyy-MM-dd"), timeSlot.start.TimeOfDay.ToString(), cmbDoctors.SelectedValue, false);
            }
            dgAppointments.ItemsSource = dt.DefaultView;
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
            if (date.Date < DateTime.Now.Date   )
            {
                MessageBox.Show("The selected date cannot be in the past.");
                return;
            }
            Priority priority = GetPriority();
            TimeOnly earliestTime = new TimeOnly(int.Parse(tbETime.Text.Split(":")[0]), int.Parse(tbETime.Text.Split(":")[1]));
            TimeOnly latestTime = new TimeOnly(int.Parse(tbLTime.Text.Split(":")[0]), int.Parse(tbLTime.Text.Split(":")[1]));
            AppointmentRequest appointmentRequest = new AppointmentRequest((Doctor)cmbDoctors.SelectedItem, earliestTime, latestTime, date, priority);
            List<TimeSlot> timeSlots = singleton.Schedule.GetClosestTimeSlots(appointmentRequest);
            dgAppointments.ItemsSource = null;
            LoadAppointmentsInDataGrid(timeSlots);
        }

        private Priority GetPriority()
        {
            if (rbDoctor.IsChecked == true) return Priority.Doctor;
            return Priority.TimeSlot;
        }

        public bool inputValidation()
        {
            if (tbETime.Text == "" || tbLTime.Text == "" || dpLDate.SelectedDate == null || cmbDoctors.SelectedItem == null || (rbDoctor.IsChecked == false && rbTimeSlot.IsChecked == false))
            {
                MessageBox.Show("Fill in all the fields");
                return false;
            }
            string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbLTime.Text, pattern) || !System.Text.RegularExpressions.Regex.IsMatch(tbETime.Text, pattern))
            {
                MessageBox.Show("Please enter a valid time value in \"HH:mm\" format.");
                return false;
            }
            return true;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
