using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

namespace ZdravoCorp.InfrastructureGroup
{
    public class Warehouse : Infrastructure
    {
        [JsonProperty("inventory")]
        private Dictionary<string, int> Inventory;
        public Warehouse(string name, Dictionary<string, int> inventory) : base(name)
        {
            this.Inventory = inventory;
        }


        public void Add(string equipment, int quantity)
        {
            if (this.Inventory.ContainsKey(equipment))
            {
                this.Inventory[equipment]+=quantity;
            }
            else
            {
                this.Inventory.Add(equipment, quantity);
            }
        }

        public Dictionary<string, int> GetInventory() { return Inventory; }
       

    }
}