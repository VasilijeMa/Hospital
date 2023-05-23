using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
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

        public static List<FunctionalItem> LoadDynamicWithHidden()
        {
            List<FunctionalItem> allPossibleCombinations = new List<FunctionalItem>();

            List<FunctionalItem> allFunctionalItems = LoadAll();
            Dictionary<string, Room> rooms = RoomRepository.LoadAll();
            Dictionary<string, EquipmentQuantity> dynamicEquipment = EquipmentRepository.LoadOnlyStaticOrDynamic(true);
            bool found = false;
            foreach (string roomName in rooms.Keys)
            {
                foreach (string equipmentName in dynamicEquipment.Keys)
                {
                    found = false;
                    foreach (FunctionalItem functionalItem in allFunctionalItems)
                    {
                        if (functionalItem.GetWhere() == roomName && functionalItem.GetWhat() == equipmentName)
                        {
                            allPossibleCombinations.Add(functionalItem);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        allPossibleCombinations.Add(new FunctionalItem(roomName, equipmentName, 0));
                    }
                }
            }
            return allPossibleCombinations;


        }
    }
}
