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

        List<String> usernames = new List<String>();
        List<int> patientsIds = new List<int>();
        List<int> recordsIds = new List<int>();

        Patient selectedPatient;
        MedicalRecord selectedRecord;

        int selectedIndex = 0;

        public CreateMedicalRecordWindow(bool createoredit, Patient selectedPatient, bool doctorornurse)
        {
            InitializeComponent();
            this.createoredit = createoredit;
            this.selectedPatient = selectedPatient;
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
            //selectedPatient = Singleton.Instance.patients[selectedIndex];

            int recordId = selectedPatient.MedicalRecordId;
            foreach (MedicalRecord record in Singleton.Instance.medicalRecords)
            {
                if (recordId == record.Id)
                {
                    selectedRecord = record;
                    break;
                }
            }

            firstname.Text = selectedPatient.FirstName;
            lastname.Text = selectedPatient.LastName;
            birthdate.Text = selectedPatient.BirthDate.ToString();
            username.Text = selectedPatient.Username;
            password.Text = selectedPatient.Password;
            height.Text = selectedRecord.Height.ToString();
            weight.Text = selectedRecord.Weight.ToString();
            anamnesis.Text = selectedRecord.Anamnesis;

            //Singleton.Instance.patients.Remove(selectedPatient);
            //Singleton.Instance.medicalRecords.Remove(selectedRecord);
        }

        private bool validateDate(string dateInString)
        {
            DateOnly temp;
            if (DateOnly.TryParse(dateInString, out temp))
            {
                return true;
            }
            return false;
        }

        private List<String> usedUsernames()
        {
            foreach (Patient patient in Singleton.Instance.patients)
            {
                usernames.Add(patient.Username);
            }
            return usernames;
        }

        private bool isValid()
        {
            if (firstname.Text.Length == 0)
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if (lastname.Text.Length == 0)
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if ((birthdate.Text.Length == 0) || !validateDate(birthdate.Text))
            {
                MessageBox.Show("Invalid input.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if ((username.Text.Length == 0) || (usedUsernames().Contains(username.Text)))
            {
                MessageBox.Show("Invalid input.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if (password.Text.Length == 0)
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if (height.Text.Length == 0)
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if (weight.Text.Length == 0)
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if (anamnesis.Text.Length == 0)
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private List<int> usedPatientsIds()
        {
            foreach (Patient patient in Singleton.Instance.patients)
            {
                patientsIds.Add(patient.Id);
            }
            return patientsIds;
        }

        private List<int> usedMedicalRecordsIds()
        {
            foreach (MedicalRecord record in Singleton.Instance.medicalRecords)
            {
                recordsIds.Add(record.Id);
            }
            return recordsIds;
        }

        private int generatePatientId()
        {
            int limit = 100;
            int newpatientid = 0;
            for (int i = 1; i < limit; i++)
            {
                if (!usedPatientsIds().Contains(i))
                {
                    newpatientid = i;
                    break;
                }
            }
            return newpatientid;
        }

        private int generateMedicalRecordId()
        {
            int limit = 100;
            int newrecordid = 0;
            for (int i = 1; i < limit; i++)
            {
                if (!usedMedicalRecordsIds().Contains(i))
                {
                    newrecordid = i;
                    break;
                }
            }
            return newrecordid;
        }

        private void clearData()
        {
            firstname.Clear();
            lastname.Clear();
            birthdate.Clear();
            username.Clear();
            password.Clear();
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
                Patient newpatient = new Patient();
                newpatient.FirstName = firstname.Text;
                newpatient.LastName = lastname.Text;
                newpatient.BirthDate = DateOnly.Parse(birthdate.Text);
                newpatient.Username = username.Text;
                newpatient.Password = password.Text;
                newpatient.Type = "patient";
                newpatient.IsBlocked = false;
                if (createoredit)
                {
                    newpatient.Id = generatePatientId();
                    newpatient.MedicalRecordId = generateMedicalRecordId();
                }
                else
                {

                    Singleton.Instance.patients.Remove(selectedPatient);
                    Singleton.Instance.medicalRecords.Remove(selectedRecord);
                    newpatient.Id = selectedPatient.Id;
                    newpatient.MedicalRecordId = selectedPatient.MedicalRecordId;
                }

                MedicalRecord newMedicalRecord = new MedicalRecord();
                newMedicalRecord.Height = double.Parse(height.Text);
                newMedicalRecord.Weight = double.Parse(weight.Text);
                newMedicalRecord.Anamnesis = anamnesis.Text;
                newMedicalRecord.Id = newpatient.MedicalRecordId;

                Singleton.Instance.patients.Add(newpatient);
                newpatient.WriteAll(Singleton.Instance.patients);
                Singleton.Instance.medicalRecords.Add(newMedicalRecord);
                newMedicalRecord.WriteAll(Singleton.Instance.medicalRecords);


                MessageBox.Show("Operation successful. We saved your changes.", "Done", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
                this.Close();

            }
        }
    }
}
