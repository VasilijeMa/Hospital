using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.EquipmentGroup;

namespace ZdravoCorp.InfrastructureGroup
{
    public class RenovationService
    {
        private RoomRepository _roomRepository;
        private WarehouseRepository _warehouseRepository;
        private RenovationRepository _renovationRepository;
        private FunctionalItemRepository _functionalItemRepository;
        public RenovationService()
        {
            _roomRepository = new RoomRepository();
            _renovationRepository = new RenovationRepository();
            _functionalItemRepository = new FunctionalItemRepository();
            _warehouseRepository = new WarehouseRepository();
        }

        public List<string> LoadAllRooms()
        {
            return _roomRepository.GetAllNames();
        }

        public bool IsRoomScheduledForRenovation(string roomName)
        {
            return _renovationRepository.IsRoomScheduledForRenovation(roomName);
        }
        public void SaveRenovation(Renovation renovation)
        {
            _renovationRepository.Add(renovation);
        }
        private void StartRenovations()
        {
            List<Renovation> notYetStartedRenovations = _renovationRepository.ExtractNotYetExecutedRenovations(true);
            foreach (Renovation renovation in notYetStartedRenovations)
            {
                renovation.HasStarted = true;
                Dictionary<string, int> removedItems = _functionalItemRepository.EmptyOutRoom(renovation.RoomName);
                _warehouseRepository.AddItems(removedItems);

                _roomRepository.Delete(renovation.RoomName);
            }
            _renovationRepository.AddAll(notYetStartedRenovations);
        }

        private void FinishRenovations()
        {
            List<Renovation> notYetFinishedRenovations = _renovationRepository.ExtractNotYetExecutedRenovations(false);
            foreach (Renovation renovation in notYetFinishedRenovations)
            {
                renovation.IsFinished = true;
                _roomRepository.MakeNewRoom(renovation.EndType);
            }
            _renovationRepository.AddAll(notYetFinishedRenovations);
        }

        public void UpdateRenovations()
        {
            StartRenovations();
            FinishRenovations();
        }

    }
}
