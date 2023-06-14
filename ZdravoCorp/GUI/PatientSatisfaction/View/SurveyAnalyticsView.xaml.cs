using System.Windows;
using ZdravoCorp.GUI.PatientSatisfaction.ViewModel;

namespace ZdravoCorp.Core.PatientSatisfaction.View
{
    /// <summary>
    /// Interaction logic for SurveyAnalyticsView.xaml
    /// </summary>
    public partial class SurveyAnalyticsView : Window
    {
        public SurveyAnalyticsView(int doctor)
        {
            SurveyAnalyticsViewModel viewModel = new SurveyAnalyticsViewModel(doctor);
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
