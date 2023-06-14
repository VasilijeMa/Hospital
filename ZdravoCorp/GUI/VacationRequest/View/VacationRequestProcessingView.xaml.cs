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
using ZdravoCorp.Core.VacationRequest.Services;
using ZdravoCorp.GUI.VacationRequest.ViewModel;

namespace ZdravoCorp.GUI.VacationRequest.View
{
    /// <summary>
    /// Interaction logic for VacationRequestProcessingView.xaml
    /// </summary>
    public partial class VacationRequestProcessingView : Window
    {
        public VacationRequestProcessingView(VacationRequestProcessingService vacationRequestProcessingService)
        {
            VacationRequestProcessingViewModel viewModel = new VacationRequestProcessingViewModel(vacationRequestProcessingService);
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
