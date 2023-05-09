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
        private Timer DynamicEquipmentUpdater;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DynamicEquipmentUpdater = new Timer(UpdateDynamicEquipment, null, TimeSpan.Zero, TimeSpan.FromMinutes(5)); // Thread timer
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DynamicEquipmentUpdater.Dispose();
        }

        private void UpdateDynamicEquipment(object state)
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
    }
}
