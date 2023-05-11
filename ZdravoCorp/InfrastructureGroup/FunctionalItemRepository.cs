using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.EquipmentGroup;

namespace ZdravoCorp.InfrastructureGroup
{
    internal class FunctionalItemRepository
    {
        public static List<FunctionalItem> LoadAll()
        {

            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/functionalItems.json");
            var json = reader.ReadToEnd();
            List<FunctionalItem> allFunctionalItems = JsonConvert.DeserializeObject<List<FunctionalItem>>(json);

            return allFunctionalItems;
        }

        public static void SaveAll(List<FunctionalItem> allRequests)
        {
            string json = JsonConvert.SerializeObject(allRequests, Formatting.Indented);

            File.WriteAllText("./../../../data/functionalItems.json", json);
        }
    }
}
