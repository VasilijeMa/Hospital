using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.EquipmentGroup
{
    public class TransferEquipmentService
    {
        public static void DynamicTransferFromWarehouseToRoom(string RoomTo)
        {

        }

        public static void DynamicTransferBetweenRooms(string RoomFrom, string RoomTo)
        {
            
        }

        public static bool SaveStaticTransferRequest(ItemCollection gridItems, bool fromWarehouse, string roomFrom, string roomTo, DateTime transferDate)
        {
            List<AlteredEquipmentQuantity> itemsForTransfer = new List<AlteredEquipmentQuantity>();
            foreach (AlteredEquipmentQuantity item in gridItems)
            {
                if (item.GetSelectedQuantity() != 0)
                {
                    itemsForTransfer.Add(item);
                }

            }
            if (itemsForTransfer.Count > 0)
            {

                StaticEquipmentTransferRequest request = new StaticEquipmentTransferRequest(false, fromWarehouse, roomFrom, roomTo, transferDate, itemsForTransfer);

                StaticEquipmentTransferRequestRepository.Save(request);
                return true;
                
            }
            return false;
        }

        public static void StaticTransferFromWarehouseToRoom(string RoomTo)
        {

        }

        public static void StaticTransferBetweenRooms(string RoomFrom, string RoomTo)
        {

        }
    }
}
