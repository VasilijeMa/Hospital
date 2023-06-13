using System.Windows;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for HospitalizationReferralView.xaml
    /// </summary>
    public partial class HospitalizationReferralView : Window
    {
        public HospitalizationReferralView(Appointment appointment)
        {
            InitializeComponent();
            DataContext = new HospitalizationReferralViewModel(appointment);
        }
    }
}
