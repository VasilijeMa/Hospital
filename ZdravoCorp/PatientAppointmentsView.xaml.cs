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
    /// Interaction logic for PatientAppointmentsView.xaml
    /// </summary>
    public partial class PatientAppointmentsView : Window
    {
        List<Anamnesis> anamneses;
        Singleton singleton;
        Patient patient;
        public PatientAppointmentsView(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            singleton = Singleton.Instance;
            anamneses = singleton.anamnesis;
            LoadAppointmentsInDataGrid();
        }

        private void LoadAppointmentsInDataGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("DoctorName", typeof(string));
            dt.Columns.Add("DoctorSpetialization", typeof(string));
            dt.Columns.Add("IsCanceled", typeof(bool));
            dt.Columns.Add("Anamnesis", typeof(string));
            foreach (Anamnesis anamnesis in anamneses)
            {
                if (anamnesis.PatientId == patient.Id)
                {
                    Appointment appointment = singleton.Schedule.GetAppointment(anamnesis.AppointmentId);
                    Doctor doctor = GetDoctor(appointment.DoctorId);
                    if (appointment.TimeSlot.start.Date > DateTime.Now.Date) continue;
                    dt.Rows.Add(appointment.Id, appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"),
                        appointment.TimeSlot.start.TimeOfDay.ToString(@"hh\:mm"), doctor.FirstName + " " + doctor.LastName,
                        doctor.Specialization.ToString(), appointment.IsCanceled, anamnesis.DoctorsObservation + "\n" + anamnesis.DoctorsConclusion);
                }
            }
            dgAppointments.ItemsSource = dt.DefaultView;
        }

        private Doctor GetDoctor(int id)
        {
            foreach(Doctor doctor in singleton.doctors)
            {
                if(doctor.Id == id)
                {
                    return doctor;
                }
            }
            return null;
        }
        private void btnOpenAnamnesis_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dgAppointments.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }

        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAppointments.SelectedItem == null)
            {
                return;
            }
            btnOpenAnamnesis.IsEnabled = true;
        }
    }
}
