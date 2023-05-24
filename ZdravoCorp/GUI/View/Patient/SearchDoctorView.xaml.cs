using System;
using System.Collections.Generic;
using System.Data;
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
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for SearchDoctorWindow.xaml
    /// </summary>
    public partial class SearchDoctorWindow : Window
    {
        public SearchDoctorWindow(Patient patient)
        {
            InitializeComponent();
            DataContext = new SearchDoctorViewModel(patient);
        }
    }
}
