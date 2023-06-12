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
using ZdravoCorp.Core.PatientSatisfaction.ViewModel;

namespace ZdravoCorp.Core.PatientSatisfaction.View
{
    /// <summary>
    /// Interaction logic for DoctorRankingsView.xaml
    /// </summary>
    public partial class DoctorRankingsView : Window
    {
        public DoctorRankingsView()
        {
            DoctorRankingsViewModel viewModel = new DoctorRankingsViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
