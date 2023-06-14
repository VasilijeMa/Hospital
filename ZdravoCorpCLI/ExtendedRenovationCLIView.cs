using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.GUI.PhysicalAssets.ViewModel;
using static System.Windows.Forms.DataFormats;

namespace ZdravoCorpCLI
{
    public class ExtendedRenovationCLIView
    {
        private RenovationViewModel _viewModel;
        public ExtendedRenovationCLIView()
        {
            int type = GetRenovationType();
            _viewModel = new RenovationViewModel((RenovationType)type);
            GetParameters();
            string error = _viewModel.ValidateConditions();
            if(error != null)
            {
                Console.WriteLine("Error: " + error);
                return;
            }
            _viewModel.SaveRenovation();
            Console.WriteLine("Renovation successfully recorded.");
        }
        private int GetRenovationType()
        {
            string option;
            Console.WriteLine("Choose option:\n1) Merge Renovation\n2) Split Renovation\n");
            while (true)
            {
                option = Console.ReadLine();
                if ((option == "1") || (option == "2")) return int.Parse(option);
                Console.WriteLine("Option not recognized, try again.");
            }
        }
        private void GetParameters()
        {
            GetDate(true);
            GetDate(false);
            GetRoomName(true);
            if (_viewModel.IsMerging)
            {
                GetRoomName(false);
            }
            GetEndType(true);
            if (_viewModel.IsSplitting)
            {
                GetEndType(false);
            }
        }
        private int GetIntInRange(int max)
        {
            int result;
            string input;
            while (true)
            {
                input = Console.ReadLine();
                try
                {
                    result = int.Parse(input);
                    if (result < 1 || result > max)
                    {
                        Console.WriteLine("Out of range, try again.");
                        continue;
                    }
                    break;
                }
                catch
                {
                    Console.WriteLine("Not an integer, try again.");
                    continue;
                }
            }
            return result-1;
        }
        private void GetDate(bool isStart)
        {
            string startOrEnd = isStart ? "start" : "end";
            string[] formats = { "dd.MM.yyyy." , "d.M.yyyy.", "d.MM.yyyy.", "dd.M.yyyy." };
            string input;
            bool isValid;
            DateTime date;
            Console.WriteLine("Choose the " + startOrEnd + " date in the following format: dd.MM.yyyy.");
            while (true)
            {
                input = Console.ReadLine();
                isValid = DateTime.TryParseExact(input, formats, null, System.Globalization.DateTimeStyles.None, out date);
                if (isValid) break;
                Console.WriteLine("An error occurred while parsing date, try again.");
            }
            if (isStart)
            {
                _viewModel.StartDate = date;
                return;
            }
            _viewModel.EndDate = date;
        }
        private void GetRoomName(bool isFirst)
        {
            string firstOrSecond = isFirst ? "" : "second ";
            Console.WriteLine("Choose " + firstOrSecond + "room by number.");
            for (int i = 0; i<_viewModel.RoomNames.Count();i++)
            {
                Console.WriteLine((i+1).ToString() + ") "+ _viewModel.RoomNames[i]);
            }
            int roomNamePosition = GetIntInRange(_viewModel.RoomNames.Count());
            if (isFirst)
            {
                _viewModel.RoomNamePosition = roomNamePosition;
                return;
            }
            _viewModel.SecondRoomNamePosition = roomNamePosition;
        }
        private void GetEndType(bool isFirst)
        {
            string firstOrSecond = isFirst ? "" : "second ";
            Console.WriteLine("Choose " + firstOrSecond + "end room type by number.");
            for (int i = 0; i < _viewModel.RoomTypes.Count(); i++)
            {
                Console.WriteLine((i + 1).ToString() + ") " + _viewModel.RoomTypes[i]);
            }
            int roomTypePosition = GetIntInRange(_viewModel.RoomTypes.Count());
            if (isFirst)
            {
                _viewModel.EndType = roomTypePosition;
                return;
            }
            _viewModel.SecondEndType = roomTypePosition;
        }
    }
}
