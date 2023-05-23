using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using ZdravoCorp.EquipmentGroup;

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for TransferEquipmentPopup.xaml
    /// </summary>
    public partial class TransferStaticEquipmentPopup : Window
    {
        public bool NoItems;
        bool FromWarehouse;
        string RoomFrom;
        string RoomTo;
        public BindingList<AlteredEquipmentQuantity> AllQuantities { get; set; }
        public TransferStaticEquipmentPopup(bool fromWarehouse, string roomFrom, string roomTo)
        {
            DataContext = this;

            FromWarehouse = fromWarehouse;
            RoomFrom = roomFrom;
            RoomTo = roomTo;
            AllQuantities = new BindingList<AlteredEquipmentQuantity>();

            RefreshDataGrid();
            InitializeComponent();

            TransferDatePicker.Value = DateTime.Now;
            TransferDescription.Content = "Transferring items from:\n" + RoomFrom + "\n\nto:\n" + RoomTo;

        }

        public void RefreshDataGrid()
        {
            NoItems = true;
            AllQuantities.Clear();
            Dictionary<string, EquipmentQuantity> equipmentOrganization = EquipmentRepository.LoadOnlyStaticOrDynamic(false);
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
            DateTime transferDate = TransferDatePicker.Value ?? DateTime.Now;
            if (transferDate <= DateTime.Now)
            {
                System.Windows.MessageBox.Show("Date of transfer must be in the future!");
            }
            else
            {
                bool wasSaveSuccessful = TransferEquipmentService.SaveStaticTransferRequest(TransferGrid.Items, FromWarehouse, RoomFrom, RoomTo, transferDate);
                if (wasSaveSuccessful)
                {
                    MessageBox.Show("Transfer request recorded.");
                    Close();
                }
                else
                {
                    MessageBox.Show("You haven't chosen any items to transfer.");
                    Close();
                }

            }
        }
    }
}
