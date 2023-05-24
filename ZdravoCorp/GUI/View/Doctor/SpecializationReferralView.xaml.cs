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
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Domain.Enums;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for SpecializationReferralView.xaml
    /// </summary>
    public partial class SpecializationReferralView : Window
    {
        Appointment appointment;
        public SpecializationReferralView(Appointment appointment)
        {
            InitializeComponent();
            cmbSpecialization.Visibility = Visibility.Hidden;
            cmbDoctor.Visibility = Visibility.Hidden;
            this.appointment = appointment;
            LoadComboBoxDoctor();
            LoadComboBoxSpecialization();
        }

        private void LoadComboBoxDoctor()
        {
            cmbDoctor.ItemsSource = Singleton.Instance.DoctorRepository.Doctors;
            cmbDoctor.ItemTemplate = (DataTemplate)FindResource("doctorTemplate");
            cmbDoctor.SelectedValuePath = "Id";
        }
        private void LoadComboBoxSpecialization()
        {
            cmbSpecialization.ItemsSource = Singleton.Instance.DoctorRepository.Doctors;
            cmbSpecialization.ItemTemplate = (DataTemplate)FindResource("specializationTemplate");
        }
        private void btnRefer_Click(object sender, RoutedEventArgs e)
        {
            Specialization? selectedSpecialization = null;
            int selectedDoctorId = -1;

            if (rbDoctor.IsChecked == false && rbSpecialization.IsChecked == false)
            {
                MessageBox.Show("You must select an option!");
                return;
            }
            if (rbSpecialization.IsChecked == true)
            {
                if (cmbSpecialization.SelectedItem == null)
                {
                    MessageBox.Show("You must select specialization!");
                }
                selectedSpecialization = ((Doctor)cmbSpecialization.SelectedItem).Specialization;

            }
            if (rbDoctor.IsChecked == true)
            {
                if (cmbDoctor.SelectedItem == null)
                {
                    MessageBox.Show("You must select doctor!");
                }
                selectedDoctorId = (int)cmbDoctor.SelectedValue;
            }
            SpecializationReferral specializationReferral = new SpecializationReferral(selectedSpecialization, selectedDoctorId);
            Examination examination = new Examination(specializationReferral);
            Singleton.Instance.ExaminationRepository.Add(examination);
            appointment.ExaminationId = examination.Id;   
        }

        private void rbSpecialization_Checked(object sender, RoutedEventArgs e)
        {
            cmbDoctor.Visibility = Visibility.Hidden;
            cmbSpecialization.Visibility = Visibility.Visible;
            cmbEmpty.Visibility = Visibility.Hidden;
        }

        private void rbDoctor_Checked(object sender, RoutedEventArgs e)
        {
            cmbDoctor.Visibility = Visibility.Visible;
            cmbSpecialization.Visibility = Visibility.Hidden;
            cmbEmpty.Visibility = Visibility.Hidden;
        }
    }
}
