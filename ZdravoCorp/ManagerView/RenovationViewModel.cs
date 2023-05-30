using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.ManagerView
{
    public class RenovationViewModel
    {
        public List<string> RoomNames { get; set; }
        public List<string> RoomTypes { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today;
        public int RoomNamePosition { get; set; } = 0;
        public int EndType { get; set; } = 0;
        public ICommand RecordRenovationCommand { get; }
        public bool IsMerging { get; set; }
        public int SecondRoomNamePosition { get; set; } = 0;
        public bool IsSplitting { get; set; }
        public int SecondEndType { get; set; } = 0;
        public bool ShouldRunCommands { get; set; } = false;

        public RenovationViewModel(RenovationType renovationType)
        {
            RenovationRecordingService renovationService = new RenovationRecordingService();
            RoomNames = renovationService.LoadAllRooms();
            RoomTypes = Room.LoadAllRoomTypes();
            IsMerging = renovationType == RenovationType.Merge;
            IsSplitting = renovationType == RenovationType.Split;
            RecordRenovationCommand = new RecordRenovationCommand(this);
        }

        private string CheckDates()
        {
            if (StartDate >= EndDate)
            {
                return "End date must be after start date.";
            }
            if (StartDate <= DateTime.Today)
            {
                return "Renovation must be scheduled in the future.";
            }
            return null;
        }

        private string CheckRoomAvailability()
        {
            RenovationRecordingService renovationService = new RenovationRecordingService();
            string error = renovationService.CheckRoomAvailability(RoomNames[RoomNamePosition], EndDate);
            if (error != null)
            {
                return error;
            }
            if (IsMerging)
            {
                return renovationService.CheckRoomAvailability(RoomNames[SecondRoomNamePosition], EndDate);
            }
            return null;
        }

        public string ValidateConditions()
        {
            bool isMergingTheSameRoom = IsMerging && RoomNamePosition == SecondRoomNamePosition;
            if (isMergingTheSameRoom)
            {
                return "Cannot merge a room with itself.";
            }
            string error = CheckDates();
            if (error != null)
            {
                return error;
            }
            return CheckRoomAvailability();
        }

        public void SaveRenovation()
        {
            RenovationRecordingService renovationService = new RenovationRecordingService();
            Renovation renovation = new Renovation(StartDate, EndDate, RoomNames[RoomNamePosition], EndType + 1);
            if (IsMerging)
            {
                renovationService.SaveRenovation(new MergeRenovation(renovation, RoomNames[SecondRoomNamePosition]));
                return;
            }
            if (IsSplitting)
            {
                renovationService.SaveRenovation(new SplitRenovation(renovation, SecondEndType + 1));
                return;
            }
            renovationService.SaveRenovation(renovation);
        }
    }
}