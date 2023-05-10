using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.InfrastructureGroup;

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
        public static Dictionary<string, EquipmentQuantity> LoadOnlyStaticOrDynamic(bool onlyDynamic)
        {
            List<Equipment> allEquipment = LoadAll();
            Dictionary<string, EquipmentQuantity> equipmentOrganization = new Dictionary<string, EquipmentQuantity>();
            foreach (Equipment equipment in allEquipment)
            {
                if (equipment.IsDynamic() == onlyDynamic)
                {
                    equipmentOrganization[equipment.GetName()] = new EquipmentQuantity(equipment.GetName(), equipment.GetTypeOfEq());
                }
            }
            
            return equipmentOrganization;
        }

        public static void LoadQuantitiesFromRoom(ref Dictionary<string, EquipmentQuantity> equipmentOrganization, string roomName)
        {
            List<FunctionalItem> allFunctionalItems = FunctionalItem.LoadAll();
            foreach (FunctionalItem item in allFunctionalItems)
            {
                if (equipmentOrganization.ContainsKey(item.GetWhat()) && item.GetWhere() == roomName)
                {
                    equipmentOrganization[item.GetWhat()].IncreaseQuantity(item.GetAmount());
                }
            }
        }

        public static void LoadQuantitiesFromWarehouse(ref Dictionary<string, EquipmentQuantity> equipmentOrganization)
        {
            Warehouse warehouse = WarehouseRepository.Load();
            foreach (string itemName in warehouse.GetInventory().Keys)
            {
                if (equipmentOrganization.ContainsKey(itemName))
                {
                    equipmentOrganization[itemName].IncreaseQuantity(warehouse.GetInventory()[itemName]);
                }
            }
        }

        public void LoadAllQuantities(ref Dictionary<string, EquipmentQuantity> equipmentOrganization)
        {

            LoadQuantitiesFromWarehouse(ref equipmentOrganization);

            List<FunctionalItem> allFunctionalItems = FunctionalItem.LoadAll();
            foreach (FunctionalItem item in allFunctionalItems)
            {
                if (equipmentOrganization.ContainsKey(item.GetWhat()))
                {
                    equipmentOrganization[item.GetWhat()].IncreaseQuantity(item.GetAmount());
                }
            }
        }
    }
}
