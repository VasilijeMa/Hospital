using System.Windows;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for HospitalTreatment.xaml
    /// </summary>
    public partial class HospitalTreatment : Window
    {

        public HospitalTreatment(HospitalStayService hospitalStayService)
        {
            InitializeComponent();
            DataContext = new HospitalTreatmentViewModel(hospitalStayService);
        }

    }
}
