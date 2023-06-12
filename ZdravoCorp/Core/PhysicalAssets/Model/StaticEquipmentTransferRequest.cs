using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.PhysicalAssets.Model
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

        public List<AlteredEquipmentQuantity> GetItemsForTransfer()
        {
            return ItemsForTransfer;
        }
        public string GetRoomFrom()
        {
            return RoomFrom;
        }

        public string GetRoomTo()
        {
            return RoomTo;
        }

        public bool IsFromWarehouse()
        {
            return FromWarehouse;
        }
        public bool IsFinished()
        {
            return Finished;
        }

        public DateTime GetTransferDate()
        {
            return TransferDate;
        }

        public void Finish()
        {
            Finished = true;
        }
    }
}
