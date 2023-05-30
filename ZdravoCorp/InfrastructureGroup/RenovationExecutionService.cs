using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.InfrastructureGroup
{
    public class RenovationExecutionService
    {
        private FunctionalItemRepository _functionalItemRepository;
        private WarehouseRepository _warehouseRepository;
        private RenovationStorageService _renovationStorageService;
        private RoomRepository _roomRepository;

        public RenovationExecutionService()
        {
            _functionalItemRepository = new FunctionalItemRepository();
            _warehouseRepository = new WarehouseRepository();
            _roomRepository = new RoomRepository();
            _renovationStorageService = new RenovationStorageService();
        }

        private void DecommissionRoom(string roomName)
        {
            Dictionary<string, int> removedItems = _functionalItemRepository.EmptyOutRoom(roomName);
            _warehouseRepository.AddItems(removedItems);
            _roomRepository.Delete(roomName);
        }
        private void StartRenovations()
        {
            List<Renovation> notYetStartedRenovations = _renovationStorageService.ExtractNotYetExecutedRenovations(true);
            if (notYetStartedRenovations.Count == 0) { return; }
            foreach (var renovation in notYetStartedRenovations)
            {
                renovation.HasStarted = true;
                DecommissionRoom(renovation.RoomName);
                if (renovation.GetType() == typeof(MergeRenovation))
                {
                    DecommissionRoom(((MergeRenovation)renovation).SecondRoomName);
                }
            }
            _renovationStorageService.AddAll(notYetStartedRenovations);
        }

        private void FinishRenovations()
        {
            List<Renovation> notYetFinishedRenovations = _renovationStorageService.ExtractNotYetExecutedRenovations(false);
            if (notYetFinishedRenovations.Count == 0) { return; }
            foreach (var renovation in notYetFinishedRenovations)
            {
                renovation.IsFinished = true;
                _roomRepository.MakeNewRoom(renovation.EndType);
                if (renovation.GetType() == typeof(SplitRenovation))
                {
                    _roomRepository.MakeNewRoom(((SplitRenovation)renovation).SecondEndType);
                }
            }
            _renovationStorageService.AddAll(notYetFinishedRenovations);
        }

        public void UpdateRenovations()
        {
            StartRenovations();
            FinishRenovations();
        }
    }
}
