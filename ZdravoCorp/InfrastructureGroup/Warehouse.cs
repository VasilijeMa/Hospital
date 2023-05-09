using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

namespace ZdravoCorp.InfrastructureGroup
{
    public class Warehouse : Infrastructure
    {
        private Dictionary<string, int> Inventory;
        public Warehouse(string name, Dictionary<string, int> inventory) : base(name)
        {
            this.Inventory = inventory;
        }

        void Add(string equipment)
        {
            if (this.Inventory.ContainsKey(equipment))
            {
                this.Inventory[equipment]++;
            }
            else
            {
                this.Inventory.Add(equipment, 1);
            }
        }

        public Dictionary<string, int> GetInventory() { return Inventory; }
        public static Warehouse Load()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/warehouse.json");
            var json = reader.ReadToEnd();
            Warehouse warehouse = JsonConvert.DeserializeObject<Warehouse>(json);
            return warehouse;
        }

    }
}