using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;
using ZdravoCorp.Core.PhysicalAssets.Services;

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for TransferDynamicEquipment.xaml
    /// </summary>
    public partial class TransferDynamicEquipment : Window
    {
        public List<string> FromOptions { get; set; }
        public List<string> ToOptions { get; set; }

        public ObservableCollection<FunctionalItem> RoomsShortOfEquipment { get; set; }
        public TransferDynamicEquipment()
        {
            DataContext = this;
            IRoomRepository roomRepository = new RoomRepository();
            Dictionary<string, Room> rooms = roomRepository.LoadAll();
            IWarehouseRepository warehouseRepository = new WarehouseRepository();
            Warehouse warehouse = warehouseRepository.Load();

            RoomsShortOfEquipment = new ObservableCollection<FunctionalItem>();
            RefreshDataGrid();

            FromOptions = new List<string>() { warehouse.GetName() };
            ToOptions = new List<string>();

            foreach (string name in rooms.Keys)
            {
                ToOptions.Add(name);
                FromOptions.Add(name);
            }

            InitializeComponent();
        }

        public void RefreshDataGrid()
        {
            RoomsShortOfEquipment.Clear();
            List<FunctionalItem> allCombinations = TransferEquipmentService.LoadDynamicFunctionalItems();

            foreach (FunctionalItem item in allCombinations)
            {
                if (item.GetAmount() < 5)
                {
                    RoomsShortOfEquipment.Add(item);
                }
            }
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

                TransferDynamicEquipmentPopup newWindow = new TransferDynamicEquipmentPopup(indexFrom == 0, nameFrom, nameTo);
                if (newWindow.NoItems)
                {
                    MessageBox.Show("There are no dynamic items to transfer in this room!");
                }
                else
                {
                    newWindow.ShowDialog();
                }

            }
        }
    }
}
