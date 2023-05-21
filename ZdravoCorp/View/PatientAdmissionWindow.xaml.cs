using System.Data;
using System.Windows;
using System.Windows.Forms;
using ZdravoCorp.Domain;
using ZdravoCorp.Servieces;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for PatientAdmissionWindow.xaml
    /// </summary>
    public partial class PatientAdmissionWindow : Window
    {
        public PatientAdmissionWindow()
        {
            InitializeComponent();
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
            foreach (Appointment appointment in Singleton.Instance.Schedule.todaysAppointments)
            {
                dt.Rows.Add(appointment.Id.ToString()
                    , appointment.TimeSlot.start.ToString()
                    , appointment.getDoctor().Username
                    , appointment.getDoctor().FirstName
                    , appointment.getDoctor().LastName
                    , appointment.getPatient().Username
                    , appointment.getPatient().FirstName
                    , appointment.getPatient().LastName);
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

            Appointment selectedAppointment = Singleton.Instance.Schedule.todaysAppointments[selectedIndex];
            AppointmentService appointmentService = new AppointmentService(selectedAppointment);
            if (!appointmentService.IsAbleToStart())
            {
                MessageBox.Show("You cannot start a appointment.");
                return;
            }
            //isAlreadyExsist(selectedAppointment.Id);
            Patient patient = selectedAppointment.getPatient();
            CreateMedicalRecordWindow medicalRecordWindow = new CreateMedicalRecordWindow(false, patient, false, selectedAppointment);
            //AnamnesisView anamnesisView = new AnamnesisView(selectedAppointment,true);
            medicalRecordWindow.ShowDialog();
        }
    }
}
