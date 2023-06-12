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
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.Core.PatientSatisfaction.ViewModel;
using ZdravoCorp.Core.PhysicalAssets.ViewModel;

namespace ZdravoCorp.Core.PatientSatisfaction.View
{
    /// <summary>
    /// Interaction logic for SurveyAnalyticsView.xaml
    /// </summary>
    public partial class SurveyAnalyticsView : Window
    {
        public SurveyAnalyticsView(string doctor)
        {
            SurveyAnalyticsViewModel viewModel = new SurveyAnalyticsViewModel(doctor);
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
