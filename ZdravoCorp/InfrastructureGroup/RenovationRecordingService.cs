using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.EquipmentGroup;

namespace ZdravoCorp.InfrastructureGroup
{
    public class RenovationRecordingService
    {
        //private AppointmentRepository _appointmentRepository
        private RoomRepository _roomRepository;
        private RenovationStorageService _renovationStorageService;
        public RenovationRecordingService()
        {
            //_appointmentRepository = new AppointmentRepository();
            _roomRepository = new RoomRepository();
            _renovationStorageService = new RenovationStorageService();
        }

        public List<string> LoadAllRooms()
        {
            return _roomRepository.GetAllNames();
        }

        public void SaveRenovation(Renovation renovation)
        {
            _renovationStorageService.SaveRenovation(renovation);
        }

        public string CheckRoomAvailability(string roomName)
        {
            if (_renovationStorageService.IsRoomScheduledForRenovation(roomName))
            {
                return "Room is already scheduled for renovation.";
            }
            //if(_appointmentRepository.IsRoomScheduledForRenovation(roomName))
            //{
            //    return "An appointment is scheduled in the room after the requested date." 
            //}
            return null;
        }

    }
}
