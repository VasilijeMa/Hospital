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

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MyAppointmentsWindow.xaml
    /// </summary>
    public partial class MyAppointmentsWindow : Window
    {
        List<Appointment> appointments;
        public MyAppointmentsWindow(Patient patient)
        {
            InitializeComponent();
            Singleton singleton = Singleton.Instance;
            appointments = singleton.Schedule.LoadAllAppointments();
            LoadAppointmentsInDataGrid(patient);
        }

        private void LoadAppointmentsInDataGrid(Patient patient)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("DoctorID", typeof(int));
            dt.Columns.Add("IsCanceled", typeof(bool));
            foreach (Appointment appointment in appointments)
            {
                if (appointment.PatientId == patient.Id)
                {
                    dt.Rows.Add(appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"), appointment.TimeSlot.start.TimeOfDay.ToString(), appointment.DoctorId, appointment.IsCanceled);
                }
            }
            dgAppointments.ItemsSource = dt.DefaultView;
        }

        private void dgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataRowView)dgAppointments.SelectedItem).Row["IsCanceled"].Equals(true))
            {
                btnCancel.IsEnabled = false;
            }
            else
            {
                btnCancel.IsEnabled = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ((DataRowView)dgAppointments.SelectedItem).Row["IsCanceled"] = true;
            btnCancel.IsEnabled = false;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
