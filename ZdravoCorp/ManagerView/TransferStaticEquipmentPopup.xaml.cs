using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            NoItems=true;
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
            foreach(EquipmentQuantity equipmentQuantity in equipmentOrganization.Values) {
                if(equipmentQuantity.Quantity > 0)
                {
                    AllQuantities.Add(new AlteredEquipmentQuantity(equipmentQuantity.GetName(), equipmentQuantity.GetQuantity()));
                    NoItems = false;
                }
            }
        }

        private void TransferClick(object sender, RoutedEventArgs e)
        {
            DateTime transferDate = TransferDatePicker.Value ?? DateTime.Now;
            if(transferDate <= DateTime.Now) {
                System.Windows.MessageBox.Show("Date of transfer must be in the future!");
            }
            else
            {
                List<AlteredEquipmentQuantity> itemsForTransfer = new List<AlteredEquipmentQuantity>();
                foreach(AlteredEquipmentQuantity item in TransferGrid.Items)
                {
                    if (item.GetSelectedQuantity() != 0)
                    {
                        itemsForTransfer.Add(item);
                    }
                    
                }
                if(itemsForTransfer.Count > 0)
                {

                    StaticEquipmentTransferRequest request = new StaticEquipmentTransferRequest(false, FromWarehouse, RoomFrom, RoomTo, transferDate, itemsForTransfer);

                    StaticEquipmentTransferRequestRepository.Save(request);
                    MessageBox.Show("Transfer request recorded.");
                    Close();
                }
                else
                {
                    MessageBox.Show("You haven't chosen any items for transfer.");
                }
                
            }
        }
    }
}
