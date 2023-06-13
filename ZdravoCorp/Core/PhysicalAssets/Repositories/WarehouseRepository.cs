using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private Warehouse _warehouse;
        public Warehouse Load()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/warehouse.json");
            var json = reader.ReadToEnd();
            Warehouse warehouse = JsonConvert.DeserializeObject<Warehouse>(json);
            return warehouse;
        }
        public WarehouseRepository()
        {
            _warehouse = Load();
        }
        public void Save()
        {
            string json = JsonConvert.SerializeObject(_warehouse, Formatting.Indented);
            File.WriteAllText("./../../../data/warehouse.json", json);
        }
        public void Save(Warehouse warehouse)
        {
            _warehouse = warehouse;
            Save();
        }
        public void AddItem(string key, int amount)
        {
            _warehouse.Add(key, amount);
        }
        public void AddItems(Dictionary<string, int> itemAmounts)
        {
            foreach (string key in itemAmounts.Keys)
            {
                AddItem(key, itemAmounts[key]);
            }
            Save();
        }
    }
}
