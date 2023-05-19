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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Domain;

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

            foreach (Patient patient in Singleton.Instance.patients)
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
            Patient selected = Patient.getById(id);
            DisplayMedicalRecord(selected);
        }

        private void SearchPatientById(object sender, RoutedEventArgs e)
        {
            if (patientIdtext.Text == "")
            {
                MessageBox.Show("Please enter id to search.");
            }
            int id = int.Parse(patientIdtext.Text);
            Patient searched = Patient.getById(id);
            DisplayMedicalRecord(searched);
        }

        private void DisplayMedicalRecord(Patient patient)
        {
            if (!doctor.IsAlreadyExamined(patient.Id))
            {
                MessageBox.Show("You cannot access the medical record.");
                return;
            }
            CreateMedicalRecordWindow medicalRecordView = new CreateMedicalRecordWindow(false, patient, true, null, true);
            medicalRecordView.ShowDialog();
        }
    }
}
