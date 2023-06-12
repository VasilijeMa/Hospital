using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.PhysicalAssets.Service;
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
        private Timer _renovator;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DynamicEquipmentAdder = new Timer(AddAndUpdateDynamicEquipment, null, TimeSpan.Zero, TimeSpan.FromMinutes(5)); // Thread timers
            StaticEquipmentMover = new Timer(TransferEquipmentService.MoveAllStaticEquipment, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            _renovator = new Timer(Renovate, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DynamicEquipmentAdder.Dispose();
            StaticEquipmentMover.Dispose();
            _renovator.Dispose();
        }
        private void Renovate(object state)
        {
            RenovationExecutionService renovationService = new RenovationExecutionService();
            renovationService.UpdateRenovations();
        }

        private void AddAndUpdateDynamicEquipment(object state)
        {
            bool anyRequestsChanged = EquipmentService.AddDynamicEquipment();

            if (anyRequestsChanged)
            {
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
