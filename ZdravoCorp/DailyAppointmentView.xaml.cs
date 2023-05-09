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
    /// Interaction logic for DailyAppointmentView.xaml
    /// </summary>
    public partial class DailyAppointmentView : Window
    {
        private List<Appointment> appointments;
        private Doctor doctor;
        Singleton singleton;

        public DailyAppointmentView(List<Appointment> appointments, Doctor doctor)
        {
            InitializeComponent();
            this.singleton = Singleton.Instance;
            this.doctor = doctor;
            this.appointments = appointments;
            loadDataGrid();
        }

        private void loadDataGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AppointmentID", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Duration", typeof(int));
            dt.Columns.Add("DoctorID", typeof(int));
            dt.Columns.Add("PatientID", typeof(int));
            dt.Columns.Add("IsCanceled", typeof(bool));
            foreach (Appointment appointment in this.appointments)
            {
                dt.Rows.Add(
                    appointment.Id,
                    appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"),
                    appointment.TimeSlot.start.TimeOfDay.ToString(),
                    appointment.TimeSlot.duration,
                    appointment.DoctorId,
                    appointment.PatientId,
                    appointment.IsCanceled
                );
            }
            this.dataGrid.ItemsSource = dt.DefaultView;
        }

        private void startAppointmentClick(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }

            int id = (int)item.Row["AppointmentId"];
            Appointment selectedAppointment = singleton.Schedule.GetAppointment(id);   // ili samo appointment ?
            Patient patient = selectedAppointment.getPatient();

            if (!selectedAppointment.isAbleToStart())
            {
                MessageBox.Show("You cannot start a appointment.");
                return;
            }

            CreateMedicalRecordWindow medicalRecord = new CreateMedicalRecordWindow(false, patient, false);
            medicalRecord.ShowDialog();
        }
    }
}

