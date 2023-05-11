using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.InfrastructureGroup
{
    public class WarehouseRepository
    {
        public static Warehouse Load()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/warehouse.json");
            var json = reader.ReadToEnd();
            Warehouse warehouse = JsonConvert.DeserializeObject<Warehouse>(json);
            return warehouse;
        }

        public static void Save(Warehouse warehouse)
        {
            string json = JsonConvert.SerializeObject(warehouse, Formatting.Indented);

            File.WriteAllText("./../../../data/warehouse.json", json);
        }
    }
}
