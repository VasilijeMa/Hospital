using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for HospitalTreatment.xaml
    /// </summary>
    public partial class HospitalTreatment : Window
    {

        public HospitalTreatment()
        {

            InitializeComponent();
            DataContext = new HospitalTreatmentViewModel();
        }

    }
}
