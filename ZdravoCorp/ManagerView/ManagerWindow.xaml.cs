using System.Windows;

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void FilterClick(object sender, RoutedEventArgs e)
        {
            FilterEquipment newWindow = new FilterEquipment();
            newWindow.ShowDialog();
        }

        private void OrderClick(object sender, RoutedEventArgs e)
        {
            OrderDynamicEquipment newWindow = new OrderDynamicEquipment();
            newWindow.ShowDialog();
        }

        private void StaticTransferClick(object sender, RoutedEventArgs e)
        {
            TransferStaticEquipment newWindow = new TransferStaticEquipment();
            newWindow.ShowDialog();
        }

        private void DynamicTransferClick(object sender, RoutedEventArgs e)
        {
            TransferDynamicEquipment newWindow = new TransferDynamicEquipment();
            newWindow.ShowDialog();
        }

        private void SimpleRenovationClick(object sender, RoutedEventArgs e)
        {
            RenovationView newWindow = new RenovationView();
            newWindow.ShowDialog();
        }
        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
