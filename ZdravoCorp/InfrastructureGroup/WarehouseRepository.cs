using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZdravoCorp.InfrastructureGroup
{
    public class WarehouseRepository
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
        private void Save()
        {
            string json = JsonConvert.SerializeObject(_warehouse, Formatting.Indented);
            File.WriteAllText("./../../../data/warehouse.json", json);
        }
        public void Save(Warehouse warehouse)
        {
            _warehouse = warehouse;
            Save();
        }
        private void AddItem(string key, int amount)
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
