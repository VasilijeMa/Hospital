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

namespace ZdravoCorp
{

    public partial class SearchPatientWindow : Window
    {
        Doctor doctor;

        public SearchPatientWindow(Doctor doctor)
        {
            InitializeComponent();
            this.doctor = doctor;
            loadDataGrid();
        }

        public void loadDataGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PatientID", typeof(int));
            dt.Columns.Add("FistName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("BirthDate", typeof(string));
            dt.Columns.Add("IsBlocked", typeof(bool));

            foreach (Patient patient in Singleton.Instance.patients)
            {
                string birthDate = patient.BirthDate.ToString("yyyy-MM-dd");
                dt.Rows.Add(patient.Id, patient.FirstName, patient.LastName, birthDate, patient.IsBlocked);
            }
            this.dataGrid.ItemsSource = dt.DefaultView;
        }

        private void getMedicalRecord(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Patient is not selected.");
            }
            int id = (int)item.Row["PatientID"];
            Patient patient = Patient.getById(id);
            displayMedicalRecord(patient);
        }

        private void searchPatientFromId(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(patientIdtext.Text);
            Patient searched = Patient.getById(id);    // jel treba ById ???
            displayMedicalRecord(searched);
        }

        private void displayMedicalRecord(Patient patient)
        {
            if (!doctor.isAlreadyExamined(patient.Id))
            {
                MessageBox.Show("You cannot access the medical record.");
                return;
            }
            CreateMedicalRecordWindow medicalRecordView = new CreateMedicalRecordWindow(false, patient, false);
            medicalRecordView.ShowDialog();
        }
    }
}
