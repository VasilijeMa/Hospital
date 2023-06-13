using Newtonsoft.Json;
using System;

namespace ZdravoCorp.Core.PhysicalAssets.Model
{
    public class DynamicEquipmentRequest
    {
        [JsonProperty("itemName")]
        public string ItemName { get; set; }
        [JsonProperty("itemQuantity")]
        public int ItemQuantity { get; set; }
        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }
        [JsonProperty("finished")]
        public bool Finished { get; set; }

        public DynamicEquipmentRequest(string itemName, int itemQuantity)
        {
            ItemName = itemName;
            ItemQuantity = itemQuantity;
            OrderDate = DateTime.Now;
            Finished = false;
        }

        [JsonConstructor]
        public DynamicEquipmentRequest(string itemName, int itemQuantity, string orderDate, bool finished)
        {
            ItemName = itemName;
            ItemQuantity = itemQuantity;
            OrderDate = DateTime.Parse(orderDate);
            Finished = finished;
        }
        public string ToString()
        {
            return ItemName + " " + ItemQuantity + " " + OrderDate.ToString() + " " + Finished.ToString();
        }

        public bool IsFinished()
        {
            return Finished;
        }

        public DateTime GetOrderDate()
        {
            return OrderDate;
        }

        public string GetItemName()
        {
            return ItemName;
        }

        public int GetItemQuantity()
        {
            return ItemQuantity;
        }
        public void Finish()
        {
            Finished = true;
        }
    }
}

