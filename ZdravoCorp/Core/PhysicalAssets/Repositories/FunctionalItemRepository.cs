﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Security;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;
using ZdravoCorp.Core.PhysicalAssets.Services;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories
{
    public class FunctionalItemRepository : IFunctionalItemRepository
    {
        private List<FunctionalItem> _functionalItems;
        public List<FunctionalItem> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/functionalItems.json");
            var json = reader.ReadToEnd();
            List<FunctionalItem> allFunctionalItems = JsonConvert.DeserializeObject<List<FunctionalItem>>(json);

            return allFunctionalItems;
        }
        public FunctionalItemRepository()
        {
            _functionalItems = LoadAll();
        }

        public FunctionalItem FindOrMakeCombination(string roomName, string equipmentName)
        {
            foreach (FunctionalItem functionalItem in _functionalItems)
            {
                if (functionalItem.GetWhere() == roomName && functionalItem.GetWhat() == equipmentName)
                {
                    return functionalItem;
                }
            }
            return new FunctionalItem(roomName, equipmentName, 0);
        }

        public void SaveAll()
        {
            string json = JsonConvert.SerializeObject(_functionalItems, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/functionalItems.json", json);
        }
        public void SaveAll(List<FunctionalItem> allRequests)
        {
            _functionalItems = allRequests;
            SaveAll();
        }

        public Dictionary<string, int> EmptyOutRoom(string roomName)
        {
            Dictionary<string, int> removedItems = new Dictionary<string, int>();
            List<FunctionalItem> remainingItems = new List<FunctionalItem>();
            foreach (FunctionalItem functionalItem in _functionalItems)
            {
                if (functionalItem.GetWhere() != roomName)
                {
                    remainingItems.Add(functionalItem);
                    continue;
                }
                removedItems.Add(functionalItem.GetWhat(), functionalItem.GetAmount());
            }
            SaveAll(remainingItems);
            return removedItems;
        }
    }
}
