using System.Windows;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for SpecializationReferralView.xaml
    /// </summary>
    public partial class SpecializationReferralView : Window
    {
        public SpecializationReferralView(Appointment appointment)
        {
            InitializeComponent();
            cmbSpecialization.Visibility = Visibility.Hidden;
            cmbDoctor.Visibility = Visibility.Hidden;
            DataContext = new SpecializationReferralViewModel(appointment);
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
