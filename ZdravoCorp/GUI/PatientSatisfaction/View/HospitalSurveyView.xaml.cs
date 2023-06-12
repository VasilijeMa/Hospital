using System.Windows;
using ZdravoCorp.GUI.PatientSatisfaction.ViewModel;

namespace ZdravoCorp.GUI.View.Patient
{
    /// <summary>
    /// Interaction logic for HospitalSurveyView.xaml
    /// </summary>
    public partial class HospitalSurveyView : Window
    {
        public HospitalSurveyView(Core.Domain.Patient patient)
        {
            InitializeComponent();
            DataContext = new HospitalSurveyViewModel(patient, this);
        }
    }
}
