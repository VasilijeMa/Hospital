using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace ZdravoCorp.EquipmentGroup
{
    public class DynamicEquipmentRequestRepository
    {
        public DynamicEquipmentRequestRepository() { }
           
        public static List<DynamicEquipmentRequest> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/dynamicEquipmentRequests.json");
            var json = reader.ReadToEnd();
            List<DynamicEquipmentRequest> allRequests = JsonConvert.DeserializeObject<List<DynamicEquipmentRequest>>(json);
            return allRequests;
        }

        public static void Save(DynamicEquipmentRequest request)
        {
            List<DynamicEquipmentRequest> allRequests = LoadAll();
            allRequests.Add(request);
            SaveAll(allRequests);   
        }

        public static void SaveAll(List<DynamicEquipmentRequest> allRequests)
        {
            string json = JsonConvert.SerializeObject(allRequests, Formatting.Indented);

            File.WriteAllText("./../../../data/dynamicEquipmentRequests.json", json);
        }
    }
}
