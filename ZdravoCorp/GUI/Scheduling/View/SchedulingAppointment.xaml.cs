using System;
using System.Collections.Generic;
using System.Globalization;
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
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for SchedulingAppointment.xaml
    /// </summary>
    public partial class SchedulingAppointment : Window
    {

        private PatientService patientService = new PatientService();
        private DoctorService doctorService = new DoctorService();
        private ScheduleService scheduleService = new ScheduleService();
        public SchedulingAppointment()
        {
            InitializeComponent();
            fillComboBoxWithPatients();
            fillComboBoxWithDoctors();
        }

        private void fillComboBoxWithDoctors() 
        {
            foreach (Doctor doctor in doctorService.GetDoctors()) 
            {
                doctors.Items.Add(doctor.Username);
            }
        }

        private bool isSelectedDoctor()
        {
            if (doctors.SelectedItem == null)
            {
                MessageBox.Show("You must select a doctor .", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private bool isSelectedPatient()
        {
            if (patients.SelectedItem == null)
            {
                MessageBox.Show("You must select a patient.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private DateTime convertToDateTime(String dateString)
        {
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime dateTime;
            bool isValid = DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return dateTime;
        }
        private bool isInPast(DateTime dateAndTime)
        {
            if (dateAndTime < DateTime.Now)
            {
                return true;
            }
            return false;
        }

        private bool isDateValid()
        {
            string input = dateAndTime.Text;
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime dateTime;
            bool isValid = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return isValid;
        }

        private void fillComboBoxWithPatients()
        {
            foreach (Patient patient in patientService.GetPatients())
            {
                patients.Items.Add(patient.Username);
            }
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInput()) 
            {
                TimeSlot timeSlot = new TimeSlot(convertToDateTime(dateAndTime.Text),15);
                Patient patient = patientService.GetByUsername(patients.SelectedItem.ToString());
                Appointment newAppointment  = scheduleService.CreateAppointment(timeSlot, doctorService.GetByUsername(doctors.SelectedItem.ToString()), patient);
                scheduleService.WriteAllAppointmens();
                MessageBox.Show("Appointment scheduled successfully.");
                this.Close();
            }
        }

        private bool IsValidInput() 
        {
            if (isSelectedPatient()&&isSelectedDoctor())
            {
                if (dateAndTime.Text.Equals("") || !isDateValid())
                {
                    MessageBox.Show("Date must be in format yyyy-MM-dd HH:mm:ss.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                    return false;
                }
                if (isInPast(convertToDateTime(dateAndTime.Text)))
                {
                    MessageBox.Show("Date is in past.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                    return false;
                }
            }
            return true;
        }
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to quit? ", "Question", MessageBoxButtons.YesNo);
            if (dialogResult.ToString() == "Yes")
            {
                this.Close();
            }
        }
    }
}
