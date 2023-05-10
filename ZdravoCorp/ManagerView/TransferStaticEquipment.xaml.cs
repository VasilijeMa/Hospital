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
using ZdravoCorp.InfrastructureGroup;

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

            Dictionary<string, Room> rooms = RoomRepository.LoadAll();
            Warehouse warehouse = WarehouseRepository.Load();

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

            if (indexFrom-1 == indexTo)
            {
                MessageBox.Show("You cannot transfer items inside one room.");
            }
            else
            {
                
                string nameFrom = RoomFrom.SelectedItem.ToString();
                string nameTo = RoomTo.SelectedItem.ToString();
                
                TransferEquipmentPopup newWindow = new TransferEquipmentPopup(false, indexFrom == 0, nameFrom, nameTo);
                newWindow.ShowDialog();
                

            }
        }
    }
}
