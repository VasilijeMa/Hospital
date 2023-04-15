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
        Singleton singleton;
        public MyAppointmentsWindow(Patient patient)
        {
            InitializeComponent();
            singleton = Singleton.Instance;
            appointments = singleton.Schedule.LoadAllAppointments();
            LoadAppointmentsInDataGrid(patient);
        }

        private void LoadAppointmentsInDataGrid(Patient patient)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("DoctorID", typeof(int));
            dt.Columns.Add("IsCanceled", typeof(bool));
            foreach (Appointment appointment in appointments)
            {
                if (appointment.PatientId == patient.Id)
                {
                    dt.Rows.Add(appointment.Id, appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"), appointment.TimeSlot.start.TimeOfDay.ToString(), appointment.DoctorId, appointment.IsCanceled);
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
            DataRowView item = dgAppointments.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the appointment?", "Congfirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                ((DataRowView)dgAppointments.SelectedItem).Row["IsCanceled"] = true;
                btnCancel.IsEnabled = false;
                singleton.Schedule.CancelAppointment((int)((DataRowView)dgAppointments.SelectedItem).Row["Id"]);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dgAppointments.SelectedItem as DataRowView;
            if(item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }
            if (item.Row["IsCanceled"].Equals(true))
            {
                MessageBox.Show("Appointment is canceled.");
                return;
            }
            int id = (int)item.Row["Id"];
            Appointment appointment = GetAppointment(id);
            UpdateWindow updateWindow = new UpdateWindow(appointment);
            updateWindow.ShowDialog();
        }

        private Appointment GetAppointment(int id)
        {
            foreach (var appointment in appointments)
            {
                if(appointment.Id == id)
                {
                    return appointment;
                }
            }
            return null;
        }
    }
}
