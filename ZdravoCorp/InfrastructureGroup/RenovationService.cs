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
        //private RoomRepository _rooms;
        private RenovationRepository _renovationRepository;

        public RenovationService()
        {
            _renovationRepository = new RenovationRepository();
        }

        public List<string> LoadAllRooms()
        {
            Dictionary<string, Room> allRooms = RoomRepository.LoadAll();  //move to RoomRepository
            return allRooms.Keys.ToList();
        }


        public bool IsRoomScheduledForRenovation(string roomName)
        {
            return _renovationRepository.IsRoomScheduledForRenovation(roomName);
        }

        public void SaveRenovation(Renovation renovation)
        {
            _renovationRepository.Add(renovation);
        }

    }
}
