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
            DynamicEquipmentAdder = new Timer(EquipmentService.AddDynamicEquipment, null, TimeSpan.Zero, TimeSpan.FromMinutes(5)); // Thread timers
            StaticEquipmentMover = new Timer(TransferEquipmentService.MoveAllStaticEquipment, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DynamicEquipmentAdder.Dispose();
            StaticEquipmentMover.Dispose();
        }
    }
}
