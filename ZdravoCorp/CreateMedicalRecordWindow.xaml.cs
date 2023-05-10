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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for CreateMedicalRecordWindow.xaml
    /// </summary>
    public partial class CreateMedicalRecordWindow : Window
    {
        bool createoredit;
        bool doctorornurse;
        Patient patient;
        MedicalRecord selectedRecord;
        Appointment selectedAppointment;
        public CreateMedicalRecordWindow(bool createoredit, Patient patient, bool doctorornurse,Appointment selectedAppointment)
        {
            InitializeComponent();
            this.createoredit = createoredit;
            this.patient = patient;
            this.selectedAppointment = selectedAppointment;
            this.doctorornurse = doctorornurse;
            if (doctorornurse)
            {
                confirm.Visibility = Visibility.Hidden;
                cancel.Visibility = Visibility.Hidden;
            }
            if (!createoredit)
            {
                LoadFields();
            }
        }

        private void LoadFields()
        {
            int recordId = patient.MedicalRecordId;

            foreach (MedicalRecord record in Singleton.Instance.medicalRecords)
            {
                if (recordId == record.Id)
                {
                    selectedRecord = record;
                    break;
                }
            }
            height.Text = selectedRecord.Height.ToString();
            weight.Text = selectedRecord.Weight.ToString();
            anamnesis.Text = selectedRecord.Anamnesis;
        }

        private bool isNumeric(String number) 
        {
            int n;
            bool isNumeric = int.TryParse(number, out n);
            return isNumeric;
        }
        private bool isValid()
        {
            if ((height.Text.Length == 0) || (weight.Text.Length == 0) || (anamnesis.Text.Length == 0))
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if (!((isNumeric(height.Text)) && isNumeric(weight.Text))) 
            {
                MessageBox.Show("Weight and height should be numbers.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void clearData()
        {
            height.Clear();
            weight.Clear();
            anamnesis.Clear();
        }

        public void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to quit? ", "Question", MessageBoxButtons.YesNo);
            if (dialogResult.ToString() == "Yes")
            {
                clearData();
                this.Close();
            }
        }

        public void confirm_Click(object sender, RoutedEventArgs e)
        {

            if (isValid())
            {
                MedicalRecord medicalRecord = createMedicalRecordObject();
                addToPatients(patient);
                addToUsers(patient);
                addToMedicalRecords(medicalRecord);
                MessageBox.Show("Operation successful. We saved your changes.", "Done", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
                this.Close();

            }
        }

        public MedicalRecord createMedicalRecordObject() {
            if (!createoredit)
            {
                Singleton.Instance.medicalRecords.Remove(selectedRecord);
           //     selectedRecord.WriteAll(Singleton.Instance.medicalRecords);
            }
            MedicalRecord newMedicalRecord = new MedicalRecord();
            newMedicalRecord.Height = double.Parse(height.Text);
            newMedicalRecord.Weight = double.Parse(weight.Text);
            newMedicalRecord.Anamnesis = anamnesis.Text;
            newMedicalRecord.Id = patient.MedicalRecordId;
            return newMedicalRecord;
        }

        public void addToPatients(Patient newPatient)
        { 
            //
            //
           // newPatient.WriteAll(Singleton.Instance.patients);
            Singleton.Instance.patients.Add(newPatient);
            newPatient.WriteAll(Singleton.Instance.patients);
        }

        public void addToMedicalRecords(MedicalRecord newMedicalRecord)
        {
            Singleton.Instance.medicalRecords.Add(newMedicalRecord);
            newMedicalRecord.WriteAll(Singleton.Instance.medicalRecords);
        }

        public void addToUsers(Patient newPatient) {
            Singleton.Instance.users.Add(new User(newPatient.Username, newPatient.Password, "patient"));
           // User.WriteAll(Singleton.Instance.users);
        }
        public void addAnamnesisClick(object sender, RoutedEventArgs e)
        {
            AnamnesisView anamnesis = new AnamnesisView(selectedAppointment,false);
            anamnesis.ShowDialog();
        }
    }
}
