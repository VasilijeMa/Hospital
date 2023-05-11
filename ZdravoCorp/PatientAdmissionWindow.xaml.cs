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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            foreach (Appointment appointment in Singleton.Instance.Schedule.todaysAppointments){
                        dt.Rows.Add(appointment.Id.ToString()
                            ,appointment.TimeSlot.start.ToString()
                            ,appointment.getDoctor().Username
                            ,appointment.getDoctor().FirstName
                            ,appointment.getDoctor().LastName
                            ,appointment.getPatient().Username
                            ,appointment.getPatient().FirstName
                            ,appointment.getPatient().LastName);
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
            if (!selectedAppointment.isAbleToStart())
            {
                MessageBox.Show("You cannot start a appointment.");
                return;
            }
            //isAlreadyExsist(selectedAppointment.Id);

            AnamnesisView anamnesisView = new AnamnesisView(selectedAppointment,true);
            anamnesisView.ShowDialog();
        }

        /*private void isAlreadyExsist(int appointmentId)
        {
            foreach (Anamnesis anamnesis in Singleton.Instance.anamnesis)
            {
                if (anamnesis.AppointmentId == appointmentId)
                {
                    MessageBox.Show("There is already an anamnesis in this appointemnt.");
                }
            }
        }*/
    }
}
