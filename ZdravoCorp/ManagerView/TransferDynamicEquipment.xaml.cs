using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            Dictionary<string, Room> rooms = RoomRepository.LoadAll();
            Warehouse warehouse = WarehouseRepository.Load();

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
            List<FunctionalItem> allCombinations = FunctionalItemRepository.LoadDynamicWithHidden();

            foreach(FunctionalItem item in allCombinations)
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
