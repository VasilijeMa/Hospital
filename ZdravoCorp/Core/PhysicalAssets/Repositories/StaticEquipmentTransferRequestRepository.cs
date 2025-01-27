﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.PhysicalAssets.Model;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories
{
    public class StaticEquipmentTransferRequestRepository
    {
        public static List<StaticEquipmentTransferRequest> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/staticEquipmentTransferRequests.json");
            var json = reader.ReadToEnd();
            List<StaticEquipmentTransferRequest> allRequests = JsonConvert.DeserializeObject<List<StaticEquipmentTransferRequest>>(json);
            return allRequests;
        }

        public static void Save(StaticEquipmentTransferRequest request)
        {
            List<StaticEquipmentTransferRequest> allRequests = LoadAll();
            allRequests.Add(request);
            SaveAll(allRequests);
        }

        public static void SaveAll(List<StaticEquipmentTransferRequest> allRequests)
        {
            string json = JsonConvert.SerializeObject(allRequests, Formatting.Indented);

            File.WriteAllText("./../../../data/staticEquipmentTransferRequests.json", json);
        }
    }
}
