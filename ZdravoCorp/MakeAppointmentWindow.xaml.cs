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

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MakeAppointmentWindow.xaml
    /// </summary>
    public partial class MakeAppointmentWindow : Window
    {
        public MakeAppointmentWindow(Patient patient)
        {
            InitializeComponent();
            //List<MyObject> items = new List<MyObject>();
            //myComboBox.ItemsSource = items;
            //myComboBox.DisplayMemberPath = "Name";
            //myComboBox.SelectedValuePath = "Id";
            Singleton singleton = Singleton.Instance;
            cmbDoctors.ItemsSource = singleton.doctors;
            //cmbDoctors.DisplayMemberPath = "{FirstName} {LastName}";
            cmbDoctors.ItemTemplate = (DataTemplate)FindResource("doctorTemplate");
            cmbDoctors.SelectedValuePath = "Id";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
