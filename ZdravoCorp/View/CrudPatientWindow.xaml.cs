using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using ZdravoCorp.Controllers;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for NurseWindow.xaml
    /// </summary>
    public partial class CrudPatientWindow : Window
    {

        List<MedicalRecord> records;
        List<Patient> patients;
        private Nurse nurse;
        private MedicalRecordController medicalRecordController;
        private PatientController patientController;
        public CrudPatientWindow(Nurse nurse)
        {
            InitializeComponent();
            medicalRecordController = new MedicalRecordController();
            patientController = new PatientController();
            this.nurse = nurse;
            LoadData();
        }

        public DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Patient ID");
            dt.Columns.Add("First name");
            dt.Columns.Add("Last name");
            dt.Columns.Add("Birth date");
            dt.Columns.Add("Username");
            dt.Columns.Add("Password");
            dt.Columns.Add("Medical Record ID");
            dt.Columns.Add("Height");
            dt.Columns.Add("Weight");
            dt.Columns.Add("Anamnesis");
            dt.AcceptChanges();
            return dt;
        }

        public void LoadData()
        {
            DataTable dt = CreateDataTable();
            records = Singleton.Instance.MedicalRecordRepository.Records;
            patients = Singleton.Instance.PatientRepository.Patients;
            foreach (Patient patient in patients)
            {
                foreach (MedicalRecord record in records)
                {
                    if (patient.MedicalRecordId == record.Id)
                    {
                        dt.Rows.Add(patient.Id, patient.FirstName, patient.LastName, patient.BirthDate,
                            patient.Username, patient.Password, record.Id, record.Height, record.Weight, record.EarlierIllnesses);
                        dt.AcceptChanges();
                    }
                }
            }
            datagrid.DataContext = dt.DefaultView;

        }
        public void createButton(object sender, RoutedEventArgs e)
        {
            //CreateMedicalRecordWindow createMedicalRecordWindow = new CreateMedicalRecordWindow(true,null,false);
            //createMedicalRecordWindow.ShowDialog();
            CreatePatientWindow createPatientWindow = new CreatePatientWindow(true, null, false);
            createPatientWindow.ShowDialog();
            LoadData();
        }

        public void updateButton(object sender, RoutedEventArgs e)
        {
            int selectedIndex = datagrid.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("You must select the patient whose account you want to edit.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);

            }
            else
            {
                Patient selectedPatient = Singleton.Instance.PatientRepository.Patients[selectedIndex];
                //CreateMedicalRecordWindow createMedicalRecordWindow = new CreateMedicalRecordWindow(false,selectedPatient , false);
                //createMedicalRecordWindow.ShowDialog();
                CreatePatientWindow createPatientWindow = new CreatePatientWindow(false, selectedPatient, false);
                createPatientWindow.ShowDialog();
                LoadData();

            }
        }
        public void deleteButton(object sender, RoutedEventArgs e)
        {
            int selectedIndex = datagrid.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("You must select the patient whose account you want to edit.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);

            }
            else
            {

                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this patient? ", "Question", MessageBoxButtons.YesNo);
                if (dialogResult.ToString() == "Yes")
                {
                    Patient selectedPatient = Singleton.Instance.PatientRepository.Patients[selectedIndex];
                    MedicalRecord selectedRecord = null;
                    int recordId = selectedPatient.MedicalRecordId;
                    foreach (MedicalRecord record in Singleton.Instance.MedicalRecordRepository.Records)
                    {
                        if (recordId == record.Id)
                        {
                            selectedRecord = record;
                            break;
                        }
                    }

                    Singleton.Instance.PatientRepository.Patients.Remove(selectedPatient);
                    Singleton.Instance.MedicalRecordRepository.Records.Remove(selectedRecord);
                    Singleton.Instance.UserRepository.RemoveUser(selectedPatient.Username);
                    Singleton.Instance.UserRepository.WriteAll();
                    patientController.WriteAll(Singleton.Instance.PatientRepository.Patients);
                    medicalRecordController.WriteAll(Singleton.Instance.MedicalRecordRepository.Records);
                    LoadData();
                }
            }

        }

    }
}
