using System.Windows;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.GUI.PatientSatisfaction.ViewModel;

namespace ZdravoCorp.GUI.View.Patient
{
    /// <summary>
    /// Interaction logic for DoctorSurveyView.xaml
    /// </summary>
    public partial class DoctorSurveyView : Window
    {
        public DoctorSurveyView(Core.Domain.Patient patient, Appointment appointment)
        {
            InitializeComponent();
            DataContext = new DoctorSurveyViewModel(patient, appointment, this);
        }
    }
}
