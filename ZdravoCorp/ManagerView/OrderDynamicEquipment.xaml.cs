using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZdravoCorp.EquipmentGroup;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.ManagerView
{
    
    /// <summary>
    /// Interaction logic for OrderDynamicEquipment.xaml
    /// </summary>
    public partial class OrderDynamicEquipment : Window
    {
        public ObservableCollection<EquipmentGridItem> AllDepletingDynamicEquipment { get; set; }

        public OrderDynamicEquipment()
        {
            DataContext = this;
            AllDepletingDynamicEquipment = new ObservableCollection<EquipmentGridItem>();

            RefreshDataGrid();

            InitializeComponent();
        }

        public void RefreshDataGrid()
        {
            Dictionary<string, EquipmentGridItem> equipmentOrganization = EquipmentRepository.LoadDynamic();
            AllDepletingDynamicEquipment.Clear();
            foreach (EquipmentGridItem item in equipmentOrganization.Values)
            {
                
                if (item.GetQuantity() <= 5)
                {
                    AllDepletingDynamicEquipment.Add(item);
                }
            }

        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            EquipmentGridItem eq = (EquipmentGridItem)((Button)e.Source).DataContext;
            OrderDynamicEquipmentPopup newWindow = new OrderDynamicEquipmentPopup(eq.GetName());
            newWindow.ShowDialog();
        }
    }
}
