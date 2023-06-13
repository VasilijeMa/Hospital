using System.Windows;
using ZdravoCorp.Core.PatientSatisfaction.Services;
using ZdravoCorp.GUI.PatientSatisfaction.ViewModel;

namespace ZdravoCorp.GUI.View.Patient
{
    /// <summary>
    /// Interaction logic for HospitalSurveyView.xaml
    /// </summary>
    public partial class HospitalSurveyView : Window
    {
        public HospitalSurveyView(Core.UserManager.Model.Patient patient, HospitalSurveyService hospitalSurveyService)
        {
            InitializeComponent();
            DataContext = new HospitalSurveyViewModel(patient, this, hospitalSurveyService);
        }
    }
}
