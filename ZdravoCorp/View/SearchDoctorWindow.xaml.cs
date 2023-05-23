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
using ZdravoCorp.Repositories;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for SearchDoctorWindow.xaml
    /// </summary>
    public partial class SearchDoctorWindow : Window
    {
        private Patient patient;
        private List<Doctor> doctors;
        private DoctorRepository doctorRepository;
        public SearchDoctorWindow(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            doctorRepository = new DoctorRepository();
            doctors = doctorRepository.Doctors;
            LoadDataGrid();
        }
        public void LoadDataGrid()
        {
            DataTable dt = AddColumns();
            foreach (Doctor doctor in doctors)
            {
                double rating = doctorRepository.GetAverageRating(doctor);
                dt.Rows.Add(doctor.Id, doctor.FirstName, doctor.LastName, doctor.Specialization, rating);
            }
            this.dataGrid.ItemsSource = dt.DefaultView;
        }
        private static DataTable AddColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DoctorId", typeof(int));
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("Specialization", typeof(string));
            dt.Columns.Add("Rating", typeof(double));
            return dt;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            doctors = doctorRepository.SearchDoctors(tbSearch.Text.Trim().ToUpper());
            LoadDataGrid();
        }
        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Doctor is not selected.");
                return;
            }
            Doctor doctor = doctorRepository.getDoctor((int)item.Row["DoctorId"]);
            MakeAppointmentWindow makeAppointmentWindow = new MakeAppointmentWindow(patient, doctor);
            makeAppointmentWindow.ShowDialog();
        }
    }
}
