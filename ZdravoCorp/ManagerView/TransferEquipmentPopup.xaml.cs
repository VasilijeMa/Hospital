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

namespace ZdravoCorp.ManagerView
{
    /// <summary>
    /// Interaction logic for TransferEquipmentPopup.xaml
    /// </summary>
    public partial class TransferEquipmentPopup : Window
    {
        bool IsDynamic;
        bool FromWarehouse;
        string NameFrom;
        string NameTo;
        public TransferEquipmentPopup(bool isDynamic, bool fromWarehouse, string nameFrom, string nameTo)
        {
            IsDynamic = isDynamic;
            FromWarehouse = fromWarehouse;
            NameFrom = nameFrom;
            NameTo = nameTo;
            InitializeComponent();
            TransferDescription.Content = "Transferring items from:\n" + NameFrom + "\n\nto:\n" + NameTo;
        }

        private void TransferClick(object sender, RoutedEventArgs e)
        {
            if (IsDynamic)
            {
                if (FromWarehouse)
                {
                    TransferEquipmentService.DynamicTransferFromWarehouseToRoom(NameTo);
                }
                else
                {
                    TransferEquipmentService.DynamicTransferBetweenRooms(NameFrom, NameTo);
                }
            }
            else
            {
                if (FromWarehouse)
                {
                    TransferEquipmentService.StaticTransferFromWarehouseToRoom(NameTo);
                }
                else
                {
                    TransferEquipmentService.StaticTransferBetweenRooms(NameFrom, NameTo);
                }
            }
        }
    }
}
