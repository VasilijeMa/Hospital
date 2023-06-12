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
using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.GUI.View.Doctor
{
    /// <summary>
    /// Interaction logic for FreeDaysView.xaml
    /// </summary>
    public partial class FreeDaysView : Window
    {
        public FreeDaysView(Core.Domain.Doctor doctor)
        {
            InitializeComponent();
            DataContext = new FreeDaysViewModel(doctor);
        }
    }
}
