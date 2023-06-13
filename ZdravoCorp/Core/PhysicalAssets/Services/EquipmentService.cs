using System;
using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;

namespace ZdravoCorp.Core.PhysicalAssets.Services
{
    public class EquipmentService
    {
        public static bool AddDynamicEquipment()
        {
            bool anyRequestsChanged = false;
            IWarehouseRepository warehouseRepository = new WarehouseRepository();
            Warehouse warehouse = warehouseRepository.Load();
            DateTime now = DateTime.Now;

            List<DynamicEquipmentRequest> allRequests = DynamicEquipmentRequestRepository.LoadAll();
            foreach (DynamicEquipmentRequest request in allRequests)
            {
                if (!request.IsFinished() && request.GetOrderDate().AddHours(24) <= now)                  // Time span of Equipment's arrival
                {
                    request.Finish();
                    warehouse.Add(request.GetItemName(), request.GetItemQuantity());
                    anyRequestsChanged = true;
                    if (request.GetItemQuantity() == 1)
                    {
                        MessageBox.Show("One copy of " + request.GetItemName() + " has arrived in the Warehouse.");
                    }
                    else
                    {
                        MessageBox.Show(request.GetItemQuantity() + " copies of " + request.GetItemName() + " have arrived in the Warehouse.");
                    }
                }
            }

            if (anyRequestsChanged)
            {
                warehouseRepository.Save(warehouse);
                DynamicEquipmentRequestRepository.SaveAll(allRequests);

            }

            return anyRequestsChanged;
        }
    }
}
