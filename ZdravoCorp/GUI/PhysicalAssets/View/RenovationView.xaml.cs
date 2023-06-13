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
using ZdravoCorp.GUI.PhysicalAssets.ViewModel;

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for SimpleRoomRenovation.xaml
    /// </summary>
    public partial class RenovationView : Window
    {
        public RenovationView(RenovationType renovationType)
        {
            RenovationViewModel viewModel = new RenovationViewModel(renovationType);
            DataContext = viewModel;
            InitializeComponent();
            viewModel.ShouldRunCommands = true;
            DataContext = viewModel;
        }

    }
}
