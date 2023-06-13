using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZdravoCorp.Core.PhysicalAssets.Model
{
    public class Warehouse : Infrastructure
    {
        [JsonProperty("inventory")]
        private Dictionary<string, int> Inventory;
        public Warehouse(string name, Dictionary<string, int> inventory) : base(name)
        {
            Inventory = inventory;
        }


        public void Add(string equipment, int quantity)
        {
            if (Inventory.ContainsKey(equipment))
            {
                Inventory[equipment] += quantity;
            }
            else
            {
                Inventory.Add(equipment, quantity);
            }
        }

        public Dictionary<string, int> GetInventory() { return Inventory; }


    }
}