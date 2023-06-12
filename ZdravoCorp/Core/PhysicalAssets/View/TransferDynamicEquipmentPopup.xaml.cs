using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repository;
using ZdravoCorp.Core.PhysicalAssets.Service;

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for TransferDynamicEquipmentPopup.xaml
    /// </summary>
    public partial class TransferDynamicEquipmentPopup : Window
    {
        public bool NoItems;
        bool FromWarehouse;
        string RoomFrom;
        string RoomTo;
        public BindingList<AlteredEquipmentQuantity> AllQuantities { get; set; }
        public TransferDynamicEquipmentPopup(bool fromWarehouse, string roomFrom, string roomTo)
        {
            DataContext = this;

            FromWarehouse = fromWarehouse;
            RoomFrom = roomFrom;
            RoomTo = roomTo;
            AllQuantities = new BindingList<AlteredEquipmentQuantity>();

            RefreshDataGrid();
            InitializeComponent();

            TransferDescription.Content = "Transferring items from:\n" + RoomFrom + "\n\nto:\n" + RoomTo;
            InitializeComponent();
        }

        public void RefreshDataGrid()
        {
            NoItems = true;
            AllQuantities.Clear();
            Dictionary<string, EquipmentQuantity> equipmentOrganization = EquipmentRepository.LoadOnlyStaticOrDynamic(true);
            if (FromWarehouse)
            {
                EquipmentRepository.LoadQuantitiesFromWarehouse(ref equipmentOrganization);
            }
            else
            {
                EquipmentRepository.LoadQuantitiesFromRoom(ref equipmentOrganization, RoomFrom);
            }
            foreach (EquipmentQuantity equipmentQuantity in equipmentOrganization.Values)
            {
                if (equipmentQuantity.Quantity > 0)
                {
                    AllQuantities.Add(new AlteredEquipmentQuantity(equipmentQuantity.GetName(), equipmentQuantity.GetQuantity()));
                    NoItems = false;
                }
            }
        }

        private void TransferClick(object sender, RoutedEventArgs e)
        {
            bool wasSaveSuccessful = TransferEquipmentService.MoveDynamicEquipment(TransferGrid.Items, FromWarehouse, RoomFrom, RoomTo);
            if (wasSaveSuccessful)
            {
                MessageBox.Show("Items transferred successfully.");

                IEnumerable<TransferDynamicEquipment> windowsForUpdate = null;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    windowsForUpdate = Application.Current.Windows.OfType<TransferDynamicEquipment>();
                });

                foreach (TransferDynamicEquipment window in windowsForUpdate)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        window.RefreshDataGrid();
                    });
                }

                Close();
            }
            else
            {
                MessageBox.Show("You haven't chosen any items to transfer.");
            }
        }
    }
}
