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

namespace ZdravoCorp.GUI.View.Patient
{
    /// <summary>
    /// Interaction logic for NotificationFormView.xaml
    /// </summary>
    public partial class NotificationFormView : Window
    {
        public NotificationFormView(Core.Domain.Patient patient, Notification notification = null)
        {
            InitializeComponent();
            DataContext = new NotificationFormViewModel(patient, this, notification);
        }
    }
}
