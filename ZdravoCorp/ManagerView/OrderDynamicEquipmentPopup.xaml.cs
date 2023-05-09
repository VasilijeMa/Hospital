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
using ZdravoCorp.EquipmentGroup;

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class OrderDynamicEquipmentPopup : Window
    {
        string ItemName { get; set; }


        public OrderDynamicEquipmentPopup(string itemName)
        {
            ItemName = itemName;
            InitializeComponent();
            ItemNameLabel.Content = "Ordering " + ItemName;
        }

        private void ConfirmOrderClick(object sender, RoutedEventArgs e)
        {
            DynamicEquipmentRequest request = new DynamicEquipmentRequest(ItemName, OrderQuantity.Value ?? 0);
            DynamicEquipmentRequestRepository.Save(request);
            Close();
        }
    }
}
