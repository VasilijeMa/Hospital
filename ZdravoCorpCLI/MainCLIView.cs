using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorpCLI
{
    public class MainCLIView
    {
        public MainCLIView()
        {
            string option = GetOption();
            switch (option)
            {
                case "1":
                    ExtendedRenovationCLIView renovationView = new ExtendedRenovationCLIView();
                    break;
                case "2":
                    RecommendingAppointmentsCLIView appointmentView = new RecommendingAppointmentsCLIView();
                    break;
            }
        }

        private string GetOption()
        {
            string option;
            while (true)
            {
                Console.WriteLine("Choose option:\n1) Extended Renovation\n2) Recommending Appointments\nX) Exit\n");
                option = Console.ReadLine();
                if ((option == "1") || (option == "2") || (option.ToLower() == "x")) return option;
            }
        }
    }
}