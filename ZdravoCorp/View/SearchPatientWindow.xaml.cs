using System;
using System.Data;
using System.Threading;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Servieces;

namespace ZdravoCorp
{

    public partial class SearchPatientWindow : Window
    {
        Doctor doctor;

        public SearchPatientWindow(Doctor doctor)
        {
            InitializeComponent();
            this.doctor = doctor;
            LoadDataGrid();
        }

        public void LoadDataGrid()
        {
            DataTable dt = AddColumns();

            foreach (Patient patient in Singleton.Instance.PatientRepository.Patients)
            {
                string birthDate = patient.BirthDate.ToString("yyyy-MM-dd");
                dt.Rows.Add(patient.Id, patient.FirstName, patient.LastName, birthDate, patient.IsBlocked);
            }
            this.dataGrid.ItemsSource = dt.DefaultView;
        }

        private static DataTable AddColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PatientID", typeof(int));
            dt.Columns.Add("FistName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("BirthDate", typeof(string));
            dt.Columns.Add("IsBlocked", typeof(bool));
            return dt;
        }

        private void GetMedicalRecord(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Patient is not selected.");
            }
            int id = (int)item.Row["PatientID"];
            Patient selected = Singleton.Instance.PatientRepository.getById(id);
            DisplayMedicalRecord(selected);
        }

        private void SearchPatientById(object sender, RoutedEventArgs e)
        {
            if (patientIdtext.Text == "")
            {
                MessageBox.Show("Please enter id to search.");
                return;
            }

            try
            {
                int id = int.Parse(patientIdtext.Text);
                Patient searched = Singleton.Instance.PatientRepository.getById(id);
                if (searched == null) throw new ArgumentNullException();
                DisplayMedicalRecord(searched);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Patient with choosen id doesn't exists.");
            }catch
            {
                MessageBox.Show("Wrong input!");
            }
        }

        private void DisplayMedicalRecord(Patient patient)
        {
            DoctorService doctorService = new DoctorService();
            if (!doctorService.IsAlreadyExamined(patient.Id, doctor.Id))
            {
                MessageBox.Show("You cannot access the medical record.");
                return;
            }
            CreateMedicalRecordWindow medicalRecordView = new CreateMedicalRecordWindow(false, patient, true, null, true);
            medicalRecordView.ShowDialog();
        }
    }
}
