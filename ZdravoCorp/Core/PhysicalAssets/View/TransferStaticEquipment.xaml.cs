using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repository;
using ZdravoCorp.Core.PhysicalAssets.Repository.Interfaces;

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for TransferStaticEquipment.xaml
    /// </summary>
    public partial class TransferStaticEquipment : Window
    {
        public List<string> FromOptions { get; set; }
        public List<string> ToOptions { get; set; }
        public TransferStaticEquipment()
        {
            DataContext = this;
            IRoomRepository roomRepository = new RoomRepository();
            Dictionary<string, Room> rooms = roomRepository.LoadAll();
            IWarehouseRepository warehouseRepository = new WarehouseRepository();
            Warehouse warehouse = warehouseRepository.Load();

            FromOptions = new List<string>() { warehouse.GetName() };
            ToOptions = new List<string>();

            foreach (string name in rooms.Keys)
            {
                ToOptions.Add(name);
                FromOptions.Add(name);
            }

            InitializeComponent();
        }

        private void ChooseItemsClick(object sender, RoutedEventArgs e)
        {
            int indexFrom = RoomFrom.SelectedIndex;
            int indexTo = RoomTo.SelectedIndex;

            if (indexFrom - 1 == indexTo)
            {
                MessageBox.Show("You cannot transfer items inside one room.");
            }
            else
            {

                string nameFrom = RoomFrom.SelectedItem.ToString();
                string nameTo = RoomTo.SelectedItem.ToString();

                TransferStaticEquipmentPopup newWindow = new TransferStaticEquipmentPopup(indexFrom == 0, nameFrom, nameTo);
                if (newWindow.NoItems)
                {
                    MessageBox.Show("There are no static items to transfer in this room!");
                }
                else
                {
                    newWindow.ShowDialog();
                }

            }
        }
    }
}
