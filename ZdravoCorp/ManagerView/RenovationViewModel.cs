using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Commands;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.ManagerView
{
    public class RenovationViewModel
    {
        public List<string> RoomNames { get; set; }
        public List<string> RoomTypes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RoomNamePosition { get; set; }
        public int EndType { get; set; }
        public ICommand RecordRenovationCommand { get; }
        public bool IsMerging { get; set; }
        public int SecondRoomNamePosition { get; set; }
        public bool IsSplitting { get; set; }
        public int SecondEndType { get; set; }
        public bool ShouldRunCommands { get; set; }

        public RenovationViewModel(int type)
        {
            RenovationRecordingService renovationService = new RenovationRecordingService();
            RoomNames = renovationService.LoadAllRooms();
            RoomTypes = Room.LoadAllRoomTypes();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            RoomNamePosition = 0;
            EndType = 0;
            IsMerging = type == 1;
            IsSplitting = type == 2;
            SecondRoomNamePosition = 0;
            SecondEndType = 0;
            ShouldRunCommands = false;
            RecordRenovationCommand = new RelayCommand(RecordRenovation, CanRecordRenovation);
        }

        private string CheckDates(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
            {
                return "End date must be after start date.";
            }
            if (startDate <= DateTime.Today)
            {
                return "Renovation must be scheduled in the future.";
            }
            return null;
        }

        private string ValidateConditions()
        {
            bool isMergingTheSameRoom = IsMerging && RoomNamePosition == SecondRoomNamePosition;
            if (isMergingTheSameRoom)
            {
                return "Cannot merge a room with itself.";
            }
            string error = CheckDates(StartDate, EndDate);
            if (error != null)
            {
                return error;
            }
            RenovationRecordingService renovationService = new RenovationRecordingService();
            error = renovationService.CheckRoomAvailability(RoomNames[RoomNamePosition]);
            if(error != null)
            {
                return error;
            }
            if (IsMerging)
            {
                return renovationService.CheckRoomAvailability(RoomNames[SecondRoomNamePosition]);
            }
            return null;
        }

        private void SaveRenovation()
        {
            RenovationRecordingService renovationService = new RenovationRecordingService();
            if (IsMerging)
            {
                MergeRenovation mergeRenovation = new MergeRenovation(StartDate, EndDate, RoomNames[RoomNamePosition], EndType + 1, RoomNames[SecondRoomNamePosition]);
                renovationService.SaveRenovation(mergeRenovation);
                return;
            }
            if (IsSplitting)
            {
                SplitRenovation splitRenovation = new SplitRenovation(StartDate, EndDate, RoomNames[RoomNamePosition], EndType + 1, SecondEndType + 1);
                renovationService.SaveRenovation(splitRenovation);
                return;
            }
            Renovation renovation = new Renovation(StartDate, EndDate, RoomNames[RoomNamePosition], EndType + 1);
            renovationService.SaveRenovation(renovation);
        }

        private void RecordRenovation(object obj)
        {
            if (!ShouldRunCommands)
            {
                return;
            }
            SaveRenovation();
            MessageBox.Show("Renovation successfully recorded.");
        }

        private bool CanRecordRenovation(object obj)
        {
            if(!ShouldRunCommands)
            {
                return true;
            }
            string error = ValidateConditions();
            if (error != null)
            {
                MessageBox.Show(error);
            }
            return error == null;
        }
    }
}