using System.Windows;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.GUI.PatientHealthcare.Pharmacy.ViewModel;

namespace ZdravoCorp.GUI.View.Doctor
{
    /// <summary>
    /// Interaction logic for PrescriptionView.xaml
    /// </summary>
    public partial class PrescriptionView : Window
    {
        public PrescriptionView(Appointment appointment)
        {
            InitializeComponent();
            DataContext = new PrescriptionViewModel(appointment);
        }
    }
}
