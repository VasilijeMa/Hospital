using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for PatientAdmissionWindow.xaml
    /// </summary>
    public partial class PatientAdmissionWindow : Window
    {
        private ScheduleService scheduleService = new ScheduleService();
        private PatientService patientService = new PatientService();
        private DoctorService doctorService = new DoctorService();
        private List<Appointment> todaysAppointments;
        public PatientAdmissionWindow()
        {
            InitializeComponent();
            todaysAppointments = scheduleService.GetTodaysAppointments();
            LoadData();
        }

        public DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Appointment ID");
            dt.Columns.Add("Timeslot");
            dt.Columns.Add("Doctor's username");
            dt.Columns.Add("Doctor's first name");
            dt.Columns.Add("Doctor's last name");
            dt.Columns.Add("Patient's username");
            dt.Columns.Add("Patient's first name");
            dt.Columns.Add("Patient's last name");
            dt.AcceptChanges();
            return dt;
        }
        public void LoadData()
        {
            DataTable dt = CreateDataTable();
            foreach (Appointment appointment in todaysAppointments)
            {
                Doctor doctor = doctorService.GetDoctor(appointment.DoctorId);
                Patient patient = patientService.GetById(appointment.PatientId);
                dt.Rows.Add(appointment.Id.ToString()
                    , appointment.TimeSlot.start.ToString()
                    , doctor.Username
                    , doctor.FirstName
                    , doctor.LastName
                    , patient.Username
                    , patient.FirstName
                    , patient.LastName);
                dt.AcceptChanges();
            }
            datagrid.DataContext = dt.DefaultView;
        }

        private void AddAnamnesis_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = datagrid.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("You must select the appointment first to add an anamnesis.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return;
            }

            Appointment selectedAppointment = todaysAppointments[selectedIndex];
            if (!selectedAppointment.IsAbleToStart())
            {
                MessageBox.Show("You cannot start a appointment.");
                return;
            }
            Patient patient = patientService.GetById(selectedAppointment.PatientId);
            CreateMedicalRecordWindow medicalRecordWindow = new CreateMedicalRecordWindow(false, patient, false, selectedAppointment);
            medicalRecordWindow.ShowDialog();
        }
    }
}
