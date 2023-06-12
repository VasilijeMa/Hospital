using System.Windows;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repository;

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
