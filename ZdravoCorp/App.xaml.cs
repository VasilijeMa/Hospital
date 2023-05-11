using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.EquipmentGroup;
using ZdravoCorp.InfrastructureGroup;
using ZdravoCorp.ManagerView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Timer DynamicEquipmentAdder;
        private Timer StaticEquipmentMover;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DynamicEquipmentAdder = new Timer(AddDynamicEquipment, null, TimeSpan.Zero, TimeSpan.FromMinutes(5)); // Thread timers
            StaticEquipmentMover = new Timer(MoveStaticEquipment, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DynamicEquipmentAdder.Dispose();
            StaticEquipmentMover.Dispose();
        }

        private void AddDynamicEquipment(object state)
        {
            bool anyRequestsChanged = false;
            Warehouse warehouse = WarehouseRepository.Load();
            DateTime now = DateTime.Now;
            List<DynamicEquipmentRequest> allRequests = DynamicEquipmentRequestRepository.LoadAll();
            foreach (DynamicEquipmentRequest request in allRequests)
            {
                if (!request.IsFinished() && request.GetOrderDate().AddHours(24) <= now)                      // Time span of Equipment's arrival
                {
                    request.Finish();
                    warehouse.Add(request.GetItemName(), request.GetItemQuantity());
                    anyRequestsChanged = true;
                    if (request.GetItemQuantity() == 1) {
                        MessageBox.Show("One copy of " + request.GetItemName() + " has arrived in the Warehouse.");
                    }
                    else
                    {
                        MessageBox.Show(request.GetItemQuantity() + " copies of " + request.GetItemName() + " have arrived in the Warehouse.");
                    }
                }
            }

            if (anyRequestsChanged)
            {
                WarehouseRepository.Save(warehouse);
                DynamicEquipmentRequestRepository.SaveAll(allRequests);

                IEnumerable<OrderDynamicEquipment> windowsForUpdate = null;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    windowsForUpdate = Application.Current.Windows.OfType<OrderDynamicEquipment>();
                });

                foreach (OrderDynamicEquipment window in windowsForUpdate)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        window.RefreshDataGrid();
                    });
                }

            }
        }

        private void MoveStaticEquipment(object state)
        {
            bool anyRequestsChanged = false;
            DateTime now = DateTime.Now;

            Warehouse warehouse = WarehouseRepository.Load();
            Dictionary<string, int> inventory = warehouse.GetInventory();

            List<FunctionalItem> functionalItems = FunctionalItemRepository.LoadAll();
            List<StaticEquipmentTransferRequest> allRequests = StaticEquipmentTransferRequestRepository.LoadAll();
            foreach(StaticEquipmentTransferRequest request in allRequests)
            {
                if (!request.IsFinished() && request.GetTransferDate() <= now)
                {
                    string roomFrom = request.GetRoomFrom();
                    string roomTo = request.GetRoomTo();

                    foreach (AlteredEquipmentQuantity equipmentQuantity in request.GetItemsForTransfer())
                    {

                        string equipmentName = equipmentQuantity.GetName();
                        int transferAmount = equipmentQuantity.GetSelectedQuantity();


                        int howManyMoved = 0;

                        if (request.IsFromWarehouse())
                        {
                            if (inventory.ContainsKey(equipmentName))
                            {
                                if (inventory[equipmentName] >= transferAmount)
                                {
                                    howManyMoved = transferAmount;
                                    inventory[equipmentName] -= howManyMoved;
                                    if (inventory[equipmentName]==0)
                                    {
                                        inventory.Remove(equipmentName);
                                    }
                                    request.Finish();
                                }
                                else
                                {
                                    howManyMoved = inventory[equipmentName];
                                    equipmentQuantity.SetSelectedQuantity(transferAmount-howManyMoved);
                                    inventory.Remove(equipmentName);
                                }

                            }
                        }
                        else
                        {
                            foreach (FunctionalItem item in functionalItems)
                            {
                                if (item.GetWhere() == roomFrom && item.GetWhat() == equipmentName)
                                {
                                    if (item.GetAmount() >= transferAmount)
                                    {
                                        howManyMoved = transferAmount;
                                        item.SetAmount(item.GetAmount() - howManyMoved);
                                        if (item.GetAmount() == 0)
                                        {
                                            functionalItems.Remove(item);
                                        }
                                        request.Finish();
                                    }
                                    else
                                    {
                                        howManyMoved = item.GetAmount();
                                        equipmentQuantity.SetSelectedQuantity(transferAmount-howManyMoved);
                                        functionalItems.Remove(item);
                                    }
                                    break;
                                }
                            }

                        }
                        if(howManyMoved > 0)
                        {
                            bool found = false;

                            foreach (FunctionalItem item in functionalItems)
                            {
                                if (item.GetWhere() == roomTo && item.GetWhat() == equipmentName)
                                {
                                    found = true;
                                    item.SetAmount(item.GetAmount() + howManyMoved);
                                }
                                break;
                            }
                            if (!found)
                            {
                                functionalItems.Add(new FunctionalItem(roomTo, equipmentName, howManyMoved));
                            }

                            anyRequestsChanged = true;
                        }
                        
                    }
                }
                
            }
            if (anyRequestsChanged)
            {
                StaticEquipmentTransferRequestRepository.SaveAll(allRequests);
                FunctionalItemRepository.SaveAll(functionalItems);
                WarehouseRepository.Save(warehouse);
            }
        }
    }
}
