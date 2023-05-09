using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.InfrastructureGroup;
using ZdravoCorp.ManagerView;

namespace ZdravoCorp.EquipmentGroup
{
    internal class EquipmentRepository
    {
        public EquipmentRepository() { }

        public static List<Equipment> LoadAll()
        {

            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/equipment.json");
            var json = reader.ReadToEnd();
            List<Equipment> allEquipment = JsonConvert.DeserializeObject<List<Equipment>>(json);
            return allEquipment;
        }

        public static Dictionary<string, EquipmentGridItem> LoadDynamic()
        {
            List<Equipment> allEquipment = LoadAll();
            Dictionary<string, EquipmentGridItem> equipmentOrganization = new Dictionary<string, EquipmentGridItem>();
            foreach (Equipment equipment in allEquipment)
            {
                if (equipment.IsDynamic())
                {
                    equipmentOrganization[equipment.GetName()] = new EquipmentGridItem(equipment.GetName(), equipment.GetTypeOfEq());
                }
            }
            Warehouse warehouse = WarehouseRepository.Load();
            foreach (string itemName in warehouse.GetInventory().Keys)
            {
                if (equipmentOrganization.ContainsKey(itemName))
                {
                    equipmentOrganization[itemName].IncreaseQuantity(warehouse.GetInventory()[itemName]);
                }
            }

            List<FunctionalItem> allFunctionalItems = FunctionalItem.LoadAll();
            foreach (FunctionalItem item in allFunctionalItems)
            {
                if (equipmentOrganization.ContainsKey(item.GetWhat()))
                {
                    equipmentOrganization[item.GetWhat()].IncreaseQuantity(item.GetAmount());
                }
            }
            return equipmentOrganization;
        }
    }
}
