using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.EquipmentGroup;

namespace ZdravoCorp.ManagerView
{

    /// <summary>
    /// Interaction logic for OrderDynamicEquipment.xaml
    /// </summary>
    public partial class OrderDynamicEquipment : Window
    {
        public ObservableCollection<EquipmentQuantity> AllDepletingDynamicEquipment { get; set; }

        public OrderDynamicEquipment()
        {
            DataContext = this;
            AllDepletingDynamicEquipment = new ObservableCollection<EquipmentQuantity>();

            RefreshDataGrid();

            InitializeComponent();
        }

        public void RefreshDataGrid()
        {
            Dictionary<string, EquipmentQuantity> equipmentOrganization = EquipmentRepository.LoadOnlyStaticOrDynamic(true);
            EquipmentRepository.LoadAllQuantities(ref equipmentOrganization);
            AllDepletingDynamicEquipment.Clear();
            foreach (EquipmentQuantity item in equipmentOrganization.Values)
            {

                if (item.GetQuantity() <= 5)
                {
                    AllDepletingDynamicEquipment.Add(item);
                }
            }

        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            EquipmentQuantity eq = (EquipmentQuantity)((Button)e.Source).DataContext;
            OrderDynamicEquipmentPopup newWindow = new OrderDynamicEquipmentPopup(eq.GetName());
            newWindow.ShowDialog();
        }
    }
}
