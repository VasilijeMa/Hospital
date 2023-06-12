using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorpCLI
{
    public class RecommendingAppointmentsCLIView
    {
        //ispis svih doktora
        //izbor doktora
        //unos najranijeg vremena
        //unos najkasnijeg vremena
        //unos najkasnijeg datuma
        //prioritet TimeSlot ili Doctor
        //Find, Submit, Cancel


        Patient patient;
        List<Appointment> recommendedAppointments;
        private DoctorService doctorService = new DoctorService();
        ScheduleService scheduleService = new ScheduleService();
        LogService logService = new LogService();
        private RecommendingAppointmentsService recommendingAppointmentsService = new RecommendingAppointmentsService();

        public RecommendingAppointmentsCLIView()
        {
            while (true)
            {

                Console.WriteLine("Choose doctor's Id.");
                PrintDoctors();
                Console.WriteLine("x Exit");
                Console.Write("Id:");
                int doctorId;
                string input = Console.ReadLine();
                if (Int32.TryParse(input, out doctorId) && doctorService.GetDoctor(doctorId) != null)
                {
                    Console.WriteLine("DoctorId: " + doctorId);
                    break;
                }
                if (input.ToLower().Equals("x")) return;
                Console.WriteLine("Wrong input, try again.");
            }

            TimeOnly earliestTime;
            while (true)
            {
                Console.Write("Earliest time(HH:mm): ");
                string input = Console.ReadLine();
                string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
                if (System.Text.RegularExpressions.Regex.IsMatch(input, pattern))
                {
                    earliestTime = new TimeOnly(int.Parse(input.Split(":")[0]), int.Parse(input.Split(":")[1]));
                    break;
                }
                Console.WriteLine("Please enter a valid time value in \"HH:mm\" format.");
            }

            while (true)
            {
                Console.Write("Latest time(HH:mm): ");
                string input = Console.ReadLine();
                string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
                if (System.Text.RegularExpressions.Regex.IsMatch(input, pattern))
                {
                    TimeOnly latestTime = new TimeOnly(int.Parse(input.Split(":")[0]), int.Parse(input.Split(":")[1]));
                    if (latestTime > earliestTime) break;
                    Console.WriteLine("Latest time is before earliest time!");
                }
                Console.WriteLine("Please enter a valid time value in \"HH:mm\" format.");
            }

            while (true)
            {
                Console.Write("Latest date(dd.MM.yyyy.): ");
                string input = Console.ReadLine();
                string[] formats = { "dd.MM.yyyy." };

                DateTime date;
                bool isValid = DateTime.TryParseExact(input, formats, null, System.Globalization.DateTimeStyles.None, out date);
                if (date.Date < DateTime.Now.Date)
                {
                    Console.WriteLine("The selected date cannot be in the past.");
                }

                if (isValid) break;
                Console.WriteLine("Invalid date format!");
            }

            Priority? priority = null;
            while (true)
            {
                Console.WriteLine("Choose priority");
                Console.WriteLine("1. Doctor");
                Console.WriteLine("2. Time slot");
                Console.WriteLine("Input number only");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        priority = Priority.Doctor;
                        break;
                    case "2":
                        priority = Priority.TimeSlot;
                        break;
                    default:
                        Console.WriteLine("Wrong input, try again.");
                        break;
                }
                if (priority != null) break;
            }
        }

        public void PrintDoctors()
        {
            List<Doctor> doctors = doctorService.GetDoctors();
            for (int i = 0; i < doctors.Count; i++)
            {
                Console.WriteLine("Id: " + doctors[i].Id + ", " + doctors[i].FirstName + " " + doctors[i].LastName + ", " + doctors[i].Specialization);
            }
        }
    }
}
