using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public RenovationViewModel()
        {
            RenovationService renovationService = new RenovationService();
            RoomNames = renovationService.LoadAllRooms();
            RoomTypes = Room.LoadAllRoomTypes();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            RoomNamePosition = 0;
            EndType = 0;
            RecordRenovationCommand = new RelayCommand(RecordRenovation, CanRecordRenovation);
        }

        string CheckDates(DateTime startDate, DateTime endDate)
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
        
        private string CheckAvailability(DateTime startDate, DateTime endDate, string roomName)
        {
            RenovationService renovationService = new RenovationService();
            bool isTaken = renovationService.IsRoomScheduledForRenovation(roomName); //change this function in Service for Extended
            if (isTaken)
            {
                return "This room is already scheduled for renovation.";
            }
            //search through appointments as well
            return null;
        }

        private void RecordRenovation(object obj)
        {
            Renovation renovation = new Renovation(StartDate, EndDate, RoomNames[RoomNamePosition], EndType+1); //different for Extended
            RenovationService renovationService = new RenovationService();
            renovationService.SaveRenovation(renovation);
            MessageBox.Show("Renovation successfully recorded.");
        }

        private bool CanRecordRenovation(object obj)
        {
            string error = CheckDates(StartDate, EndDate);
            if (error == null)
            {
                error = CheckAvailability(StartDate, EndDate, RoomNames[RoomNamePosition]);
            }
            if (error != null)
            {
                MessageBox.Show(error);
                return false;
            }
            return true;
        }
    }
}
