using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using ZdravoCorp.Domain;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for CreatePatientWindow.xaml
    /// </summary>
    public partial class CreatePatientWindow : Window
    {
        bool createoredit;
        List<String> usernames = new List<String>();
        List<int> patientsIds = new List<int>();
        List<int> recordsIds = new List<int>();
        Patient selectedPatient;
        bool doctorornurse;
        public CreatePatientWindow(bool createoredit, Patient selectedPatient, bool doctorornurse)
        {
            InitializeComponent();
            this.createoredit = createoredit;
            this.selectedPatient = selectedPatient;
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
            foreach (User user in Singleton.Instance.users)
            {
                if (selectedPatient != null && selectedPatient.Username == user.Username) continue;
                usernames.Add(user.Username);
            }
            return usernames;
        }

        private void LoadFields()
        {
            firstname.Text = selectedPatient.FirstName;
            lastname.Text = selectedPatient.LastName;
            birthdate.Text = selectedPatient.BirthDate.ToString();
            username.Text = selectedPatient.Username;
            password.Text = selectedPatient.Password;
        }
        private void clearData()
        {
            firstname.Clear();
            lastname.Clear();
            birthdate.Clear();
            username.Clear();
            password.Clear();
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


        private bool isValid()
        {
            if ((firstname.Text.Length == 0) || (lastname.Text.Length == 0) || (birthdate.Text.Length == 0)
                || (username.Text.Length == 0) || (password.Text.Length == 0))
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if (!validateDate(birthdate.Text))
            {
                MessageBox.Show("Date is in invalid input.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if (usedUsernames().Contains(username.Text))
            {
                MessageBox.Show("Username is used by another user.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (isValid())
            {
                Patient newpatient = CreatePatientObject();
                CreateMedicalRecordWindow createMedicalRecordWindow = new CreateMedicalRecordWindow(createoredit, newpatient, doctorornurse, null);
                createMedicalRecordWindow.ShowDialog();
                this.Close();
            }
        }
        public Patient CreatePatientObject()
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
                // Singleton.Instance.patients.Remove(selectedPatient);
                // User.RemoveUser(selectedPatient.Username);
                newpatient.Id = selectedPatient.Id;
                newpatient.MedicalRecordId = selectedPatient.MedicalRecordId;
            }
            return newpatient;
        }

    }
}
