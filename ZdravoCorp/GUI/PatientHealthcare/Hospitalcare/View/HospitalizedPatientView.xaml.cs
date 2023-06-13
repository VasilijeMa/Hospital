using System.Windows;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;

namespace ZdravoCorp.GUI.View.Doctor
{
    /// <summary>
    /// Interaction logic for HospitalizedPatientView.xaml
    /// </summary>
    public partial class HospitalizedPatientView : Window
    {
        public HospitalizedPatientView(Core.Domain.Doctor doctor)
        {
            InitializeComponent();
            DataContext = new HospitalizedPatientViewModel(doctor);
        }
    }
}
