using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;
using ZdravoCorp.Core.PhysicalAssets.Services;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories
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
            IFunctionalItemRepository functionalItemRepository = new FunctionalItemRepository();
            List<FunctionalItem> allFunctionalItems = functionalItemRepository.LoadAll();
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
            IWarehouseRepository warehouseRepository = new WarehouseRepository();
            Warehouse warehouse = warehouseRepository.Load();
            foreach (string itemName in warehouse.GetInventory().Keys)
            {
                if (equipmentOrganization.ContainsKey(itemName))
                {
                    equipmentOrganization[itemName].IncreaseQuantity(warehouse.GetInventory()[itemName]);
                }
            }
        }

        public static void LoadAllQuantities(ref Dictionary<string, EquipmentQuantity> equipmentOrganization)
        {

            LoadQuantitiesFromWarehouse(ref equipmentOrganization);
            IFunctionalItemRepository functionalItemRepository = new FunctionalItemRepository();
            List<FunctionalItem> allFunctionalItems = functionalItemRepository.LoadAll();
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
