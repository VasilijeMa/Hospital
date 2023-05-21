using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ZdravoCorp.EquipmentGroup;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for FilterEquipment.xaml
    /// </summary>
    public partial class FilterEquipment : Window
    {
        public List<Equipment> AllEquipment;
        public Dictionary<string, Room> AllRooms;
        public List<FunctionalItem> AllFunctionalItems;
        public Warehouse AllStoredItems;
        public Dictionary<string, EquipmentQuantity> EquipmentOrganization;
        public ObservableCollection<EquipmentQuantity> EquipmentGridItems { get; set; }

        public List<string> ByRoomTypeOptions { get; set; }
        public List<string> ByEquipmentTypeOptions { get; set; }
        public List<string> ByQuantityOptions { get; set; }
        public List<string> NotInWarehouseOptions { get; set; }

        void refreshGrid()
        {
            EquipmentGridItems.Clear();
            var sortedOrganization = (EquipmentOrganization.OrderByDescending(x => x.Value.Quantity)).ToDictionary(x => x.Key, x => x.Value); ;
            foreach (EquipmentQuantity eq in sortedOrganization.Values) EquipmentGridItems.Add(eq);
        }
        public FilterEquipment()
        {
            DataContext = this;

            ByRoomTypeOptions = new List<string>() { "All" };
            foreach (RoomType rt in Enum.GetValues(typeof(RoomType)))
            {
                ByRoomTypeOptions.Add(Room.GetTypeDescription(rt));
            }
            ByRoomTypeOptions.Add("Warehouse");

            ByEquipmentTypeOptions = new List<string>() { "All" };
            foreach (EquipmentType et in Enum.GetValues(typeof(EquipmentType)))
            {
                ByEquipmentTypeOptions.Add(et.ToString());
            }

            ByQuantityOptions = new List<string>() { "All", "OutOfStock", "0-10", "10+" };

            NotInWarehouseOptions = new List<string>() { "All", "NotInWarehouse" };

            AllEquipment = EquipmentRepository.LoadAll();
            AllRooms = RoomRepository.LoadAll();
            AllFunctionalItems = FunctionalItemRepository.LoadAll();
            AllStoredItems = WarehouseRepository.Load();

            EquipmentOrganization = new Dictionary<string, EquipmentQuantity>();
            EquipmentGridItems = new ObservableCollection<EquipmentQuantity>();

            FilterGridItems(0, 0, 0, 0, 0, "");


            InitializeComponent();

        }

        bool IsValidForInsert(Equipment eq, int byEquipmentType, int notInWarehouse)
        {                                                                               //                                                                                //
            return (byEquipmentType == 0 || byEquipmentType == (int)(eq.GetTypeOfEq())) && !(notInWarehouse == 1 && AllStoredItems.GetInventory().ContainsKey(eq.GetName()));
        }                                                                               //                                                                                //

        bool IsValidForIncrease(FunctionalItem fi, int byRoomType)
        {
            return (byRoomType == 0 || byRoomType == (int)(AllRooms[fi.GetWhere()].GetTypeOfRoom()));
        }

        bool IsOfValidQuantity(EquipmentQuantity egi, int byQuantity)
        {
            return (byQuantity == 1 && egi.GetQuantity() == 0) || (byQuantity == 2 && egi.GetQuantity() <= 10) || (byQuantity == 3 && egi.GetQuantity() > 10);
        }

        bool IsSearchReturn(EquipmentQuantity egi, string searchInput)
        {
            string upperInput = searchInput.ToUpper();
            string upperName = egi.GetName().ToUpper();
            string upperType = egi.GetTypeOfEq().ToUpper();
            for (int i = 0; i < upperInput.Length; i++)
            {
                char c = upperInput[i];
                if (c != ' ')
                {
                    bool exists = false;
                    for (int j = 0; j < upperName.Length; j++)
                    {
                        if (upperName[j] == c)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        for (int j = 0; j < upperType.Length; j++)
                        {
                            if (upperType[j] == c)
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (!exists)
                        {
                            return false;
                        }
                    }


                }
            }
            return true;
        }
        void FilterGridItems(int byRoomTypeLen, int byRoomType, int byEquipmentType, int byQuantity, int notInWarehouse, string searchInput)
        {
            EquipmentOrganization.Clear();
            foreach (Equipment eq in AllEquipment)
            {
                if (IsValidForInsert(eq, byEquipmentType, notInWarehouse))
                {
                    EquipmentOrganization[eq.GetName()] = new EquipmentQuantity(eq.GetName(), eq.GetTypeOfEq());
                }

            }
            if (byRoomType != byRoomTypeLen - 1)
            {
                foreach (FunctionalItem fi in AllFunctionalItems)
                {
                    if (IsValidForIncrease(fi, byRoomType) && EquipmentOrganization.ContainsKey(fi.GetWhat()))
                    {
                        EquipmentOrganization[fi.GetWhat()].IncreaseQuantity(fi.GetAmount());
                    }

                }
            }

            if (notInWarehouse == 0 && (byRoomType == 0 || byRoomType == byRoomTypeLen - 1))
            {
                foreach (string key in AllStoredItems.GetInventory().Keys)
                {
                    if (EquipmentOrganization.ContainsKey(key))
                    {
                        EquipmentOrganization[key].IncreaseQuantity(AllStoredItems.GetInventory()[key]);
                    }
                }
            }
            if (byQuantity != 0)
            {
                foreach (string key in EquipmentOrganization.Keys)
                {
                    if (!IsOfValidQuantity(EquipmentOrganization[key], byQuantity))
                    {
                        EquipmentOrganization.Remove(key);
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchInput))
            {
                foreach (string key in EquipmentOrganization.Keys)
                {
                    if (!IsSearchReturn(EquipmentOrganization[key], searchInput))
                    {
                        EquipmentOrganization.Remove(key);
                    }
                }
            }
            refreshGrid();


        }

        private void FilterButtonClick(object sender, RoutedEventArgs e)
        {

            FilterGridItems(ByRoomType.Items.Count, ByRoomType.SelectedIndex, ByEquipmentType.SelectedIndex, ByQuantity.SelectedIndex, NotInWarehouse.SelectedIndex, SearchBox.Text);


        }


    }
}
