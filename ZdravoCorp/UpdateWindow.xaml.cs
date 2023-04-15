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
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        Singleton singleton;
        public UpdateWindow(Appointment appointment)
        {
            InitializeComponent();
            singleton = Singleton.Instance;
            tbId.Text = appointment.Id.ToString();
            dpDate.SelectedDate = appointment.TimeSlot.start.Date;
            tbTime.Text = appointment.TimeSlot.start.ToString("hh:mm");
            cmbDoctors.ItemsSource = singleton.doctors;
            cmbDoctors.ItemTemplate = (DataTemplate)FindResource("doctorTemplate");
            cmbDoctors.SelectedValuePath = "Id";
            cmbDoctors.SelectedValue = appointment.DoctorId;
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
