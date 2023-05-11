using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Markup;
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
        bool startAppointment;
        Patient patient;
        MedicalRecord selectedRecord;
        Appointment selectedAppointment;


        public CreateMedicalRecordWindow(bool createoredit, Patient patient, bool doctorornurse, Appointment selectedAppointment=null, bool startAppointment=false)
        {
            InitializeComponent();
            this.createoredit = createoredit;
            this.patient = patient;
            this.selectedAppointment = selectedAppointment;
            this.doctorornurse = doctorornurse;
            this.startAppointment = startAppointment;

            if (startAppointment)
            {
                addAnamnesis.Visibility = Visibility.Hidden;
            }
            if (doctorornurse)
            {
                addAnamnesis.Visibility = Visibility.Hidden;
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
            foreach (string oneAnamnesis in selectedRecord.EarlierIllnesses)
            {
                anamnesis.Text += oneAnamnesis + ", ";
            }
            foreach (string oneAllergen in selectedRecord.Allergens)
            {
                allergen.Text += oneAllergen + ", ";
            }

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
            if (!(isDouble(height.Text) && isDouble(weight.Text))) 
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
                selectedRecord.WriteAll(Singleton.Instance.medicalRecords);
            }
            MedicalRecord newMedicalRecord = new MedicalRecord();
            newMedicalRecord.Height = double.Parse(height.Text);
            newMedicalRecord.Weight = double.Parse(weight.Text);
            newMedicalRecord.EarlierIllnesses.Add(anamnesis.Text);
            newMedicalRecord.Allergens.Add(allergen.Text);
            newMedicalRecord.Id = patient.MedicalRecordId;
            return newMedicalRecord;
        }

        public void addToPatients(Patient newPatient)
        {
            if (!createoredit)
            {
                Singleton.Instance.patients.Remove(patient);
                patient.WriteAll(Singleton.Instance.patients);
                User.RemoveUser(patient.Username);
                User.WriteAll(Singleton.Instance.users);
            }

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
            User.WriteAll(Singleton.Instance.users);
        }
        public void addAnamnesisClick(object sender, RoutedEventArgs e)
        {
            Anamnesis findAnamnesis = findAnamnesisById(selectedAppointment);
            if (findAnamnesis == null)
            {
                MessageBox.Show("The patient must first check in with the nurse.");
                return;
            }
            AnamnesisView view = new AnamnesisView(selectedAppointment, false);
            view.ShowDialog();
            LoadFields();
        }

        public bool isDouble(string data)
        {
            double d;
            if (Double.TryParse(data, out d))
            {
                return true;
            }
            return false;
        }

        public Anamnesis findAnamnesisById(Appointment selectedAppointment)
        {
            foreach (Anamnesis anamnesis in Singleton.Instance.anamnesis)
            {
                if (anamnesis.AppointmentId == selectedAppointment.Id)
                {
                    return anamnesis;
                }
            }
            return null;
        }
    }
}
