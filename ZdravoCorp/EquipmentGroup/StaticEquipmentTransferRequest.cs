using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.EquipmentGroup
{
    public class StaticEquipmentTransferRequest
    {
        [JsonProperty("finished")]
        public bool Finished { get; set; }

        [JsonProperty("fromWarehouse")]
        public bool FromWarehouse { get; set; }
        
        [JsonProperty("roomFrom")]
        public string RoomFrom { get; set; }

        [JsonProperty("roomTo")]
        public string RoomTo { get; set; }
        
        [JsonProperty("transferDate")]
        public DateTime TransferDate { get; set; }

        [JsonProperty("itemsForTransfer")]
        public List<AlteredEquipmentQuantity> ItemsForTransfer { get; set; }

        [JsonConstructor]
        public StaticEquipmentTransferRequest(bool finished, bool fromWarehouse, string roomFrom, string roomTo, string transferDate, List<AlteredEquipmentQuantity> itemsForTransfer)
        {
            Finished = finished;
            FromWarehouse = fromWarehouse;
            RoomFrom = roomFrom;
            RoomTo = roomTo;
            TransferDate = DateTime.Parse(transferDate);
            ItemsForTransfer = itemsForTransfer;
        }

        public StaticEquipmentTransferRequest(bool finished, bool fromWarehouse, string roomFrom, string roomTo, DateTime transferDate, List<AlteredEquipmentQuantity> itemsForTransfer)
        {
            Finished = finished;
            FromWarehouse = fromWarehouse;
            RoomFrom = roomFrom;
            RoomTo = roomTo;
            TransferDate = transferDate;
            ItemsForTransfer = itemsForTransfer;
        }
    }
}
