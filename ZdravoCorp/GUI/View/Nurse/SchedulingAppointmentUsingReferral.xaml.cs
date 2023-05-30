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
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.Core.Domain;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Globalization;
using static Xceed.Wpf.Toolkit.Calculator;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for SchedulingAppointmentUsingReferral.xaml
    /// </summary>
    public partial class SchedulingAppointmentUsingReferral : Window
    {
        private SpecializationReferralService specializationReferralService = new SpecializationReferralService();
        private PatientService patientService = new PatientService();
        private ExaminationService examinationService = new ExaminationService();
        private ScheduleService scheduleService = new ScheduleService();
        private DoctorService doctorService = new DoctorService();
        public SchedulingAppointmentUsingReferral()
        {

            InitializeComponent();
            fillComboBoxWithPatients();
        }

        private void fillComboBoxWithPatients() {
            foreach (Patient patient in patientService.GetPatients())
            {
                patients.Items.Add(patient.Username);
            }
        }

        private bool isSelectedPatient()
        {
            if (patients.SelectedItem == null) {
                MessageBox.Show("You must select the patient.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (isValidInput()) 
            {
                TimeSlot timeSlot = new TimeSlot(convertToDateTime(dateAndTime.Text), GetDuration());
                Patient patient = patientService.GetByUsername(patients.SelectedItem.ToString());
                Examination examination = examinationService.GetExaminationById(int.Parse(patientsExaminations.SelectedItem.ToString()));
                Appointment newAppointment = null;
                if (examination.SpecializationRefferal.Specialization != null)
                {
                    Doctor doctor = doctorService.GetSpecializedDoctor(examination.SpecializationRefferal.Specialization.ToString(), convertToDateTime(dateAndTime.Text).AddDays(-5), convertToDateTime(dateAndTime.Text).AddDays(5));
                    newAppointment = scheduleService.CreateAppointment(timeSlot, doctor, patient);
                }
                else 
                {
                    newAppointment = scheduleService.CreateAppointment(timeSlot, doctorService.GetDoctor(examination.SpecializationRefferal.DoctorId), patient);
                } 
                examination.SpecializationRefferal.IsUsed = true;
                examinationService.WriteAll();
                scheduleService.WriteAllAppointmens();
                MessageBox.Show("Appointment scheduled successfully.");
                this.Close();
            }
        }

        private int GetDuration() 
        {
            if (isOperationSelected()) 
            {
                return int.Parse(operationDuration.Text);
            }
            return 15;
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

        private bool isValidInput() 
        {
            if (isSelectedRefferal()&&isSelectedCriteria()) 
            {
                if (isOperationSelected() && operationDuration.Text.Equals(""))
                {
                    MessageBox.Show("You must type operation duration.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                    return false;
                }
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

        private bool isOperationSelected() 
        {
            if (examinationOrOperation.SelectedItem.ToString().Equals("Operation"))
            {
                return true;
            }
            return false;
        }

        private bool isSelectedCriteria() 
        {
            if (examinationOrOperation.SelectedItem == null)
            {
                MessageBox.Show("You must select examination or operation.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private bool isDateValid() 
        {
            string input = dateAndTime.Text;
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime dateTime;
            bool isValid = DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return isValid;
        }
        private bool isSelectedRefferal() 
        {
            if (patients.SelectedItem == null)
            {
                MessageBox.Show("You must select examination id.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private void fillComboBoxWithRefferals(List<int> examinationIds) 
        {
            foreach(int id in examinationIds) 
            { 
                patientsExaminations.Items.Add(id);
            }
        }

        private void fillComboBoxWithCriteria() 
        {
            examinationOrOperation.Items.Add("Examination");
            examinationOrOperation.Items.Add("Operation");
        }
        private void showReferrals_Click(object sender, RoutedEventArgs e)
        {
            if (isSelectedPatient())
            {

                MessageBox.Show(patients.SelectedItem.ToString(), "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                List<int> examinationsIds = examinationService.GetExaminationsIdsByPatient(patients.SelectedItem.ToString());
                if (examinationsIds.Count()!=0)
                {
                    fillComboBoxWithRefferals(examinationsIds);
                    fillComboBoxWithCriteria();
                }
                else
                {
                    MessageBox.Show("This patient doesn't have any examination.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                }
            }
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
