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

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for FilterEquipment.xaml
    /// </summary>
    public partial class FilterEquipment : Window
    {
        public Dictionary<string, Equipment> AllEquipment;
        public Dictionary<string, Room> AllRooms;
        public List<FunctionalItem> AllFunctionalItems;
        public Warehouse AllStoredItems;
        public Dictionary<string, EquipmentGridItem> EquipmentOrganization;
        public ObservableCollection<EquipmentGridItem> EquipmentGridItems { get; set; }
        public FilterEquipment()
        {
            AllEquipment = Equipment.LoadAll();
            AllRooms = Room.LoadAll();
            AllFunctionalItems = FunctionalItem.LoadAll();
            AllStoredItems = Warehouse.Load();

            EquipmentOrganization = new Dictionary<string, EquipmentGridItem>();
            EquipmentOrganization["Sus"] = new EquipmentGridItem("Sus", EquipmentType.Hallway);
            EquipmentOrganization["Amogus"] = new EquipmentGridItem("Amogus", EquipmentType.Surgery);
            EquipmentOrganization["BigChungus"] = new EquipmentGridItem("BigChungus", EquipmentType.Examination);

            EquipmentGridItems = new ObservableCollection<EquipmentGridItem>();
            foreach (EquipmentGridItem eq in EquipmentOrganization.Values)
            {
                EquipmentGridItems.Add(eq);
            }

            DataContext = this;

            InitializeComponent();

        }

        void LoadQuantities(int byRoomType, int byEquipmentType, int byQuantity, int notInWarehouse, string searchInput)
        {

        }

        private void FilterButtonClick(object sender, RoutedEventArgs e)
        {
            EquipmentOrganization.Clear();
            EquipmentOrganization["Lol"] = new EquipmentGridItem("Lol", EquipmentType.Furniture);
            EquipmentOrganization["HeheBoi"] = new EquipmentGridItem("HeheBoi", EquipmentType.Furniture);
            EquipmentOrganization["Hah"] = new EquipmentGridItem("Hah", EquipmentType.Furniture);

            EquipmentGridItems.Clear();
            foreach (EquipmentGridItem eq in EquipmentOrganization.Values) EquipmentGridItems.Add(eq);

        }

    }
}
