using System;
using System.Collections.Generic;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Scheduling.Repositories;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;

namespace ZdravoCorp.Core.PhysicalAssets.Services
{
    public class RenovationRecordingService
    {
        private IScheduleRepository _scheduleRepository;
        private IRoomRepository _roomRepository;
        private RenovationStorageService _renovationStorageService;
        public RenovationRecordingService()
        {
            _scheduleRepository = new ScheduleRepository();
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

        public string CheckRoomAvailability(string roomName, DateTime endDate)
        {
            if (_renovationStorageService.IsRoomScheduledForRenovation(roomName))
            {
                return "Room is already scheduled for renovation.";
            }
            if (_scheduleRepository.IsRoomScheduledForAppointment(roomName, endDate))
            {
                return "An appointment is scheduled in the room after the requested date.";
            }
            return null;
        }

    }
}
