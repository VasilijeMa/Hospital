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
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for SimpleRoomRenovation.xaml
    /// </summary>
    public partial class RenovationView : Window
    {
        public RenovationView(int type)
        {
            RenovationViewModel viewModel = new RenovationViewModel(type);
            DataContext = viewModel;
            InitializeComponent();
            viewModel.ShouldRunCommands = true;
            DataContext = viewModel;
        }

    }
}
