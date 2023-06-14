using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;

namespace ZdravoCorpCLI
{
    public class RecommendingAppointmentsCLIView
    {
        Patient patient;
        List<Appointment> recommendedAppointments;
        private DoctorService doctorService = new DoctorService();
        private PatientService patientService = new PatientService();
        private RecommendingAppointmentsCLIViewModel viewModel = new RecommendingAppointmentsCLIViewModel();
        public RecommendingAppointmentsCLIView()
        {
            RecommendAppointment();
        }

        private void RecommendAppointment()
        {
            int patientId;
            int doctorId;
            TimeOnly earliestTime;
            TimeOnly latestTime;
            DateTime date;
            Priority? priority = null;
            if (GetPatient(out patientId) || GetDoctor(out doctorId) || GetEarliestTime(out earliestTime) ||
                GetLatestTime(earliestTime, out latestTime) || GetDate(out date) || GetPriority(ref priority)) return;

            AppointmentRequest appointmentRequest = new AppointmentRequest(doctorService.GetDoctor(doctorId),
                earliestTime, latestTime, date, (Priority)priority);
            FoundAndMakeAppointment(appointmentRequest, patientId);
        }

        private void FoundAndMakeAppointment(AppointmentRequest appointmentRequest, int patientId)
        {
            recommendedAppointments = viewModel.FoundAppointments(appointmentRequest, patientId);
            MakeAppointment();
        }

        private void MakeAppointment()
        {
            int appointmentId;
            if (recommendedAppointments.Count() == 1)
            {
                viewModel.AddAppointment(recommendedAppointments[0], patient);
                Console.WriteLine("Appointment " + recommendedAppointments[0]);
            }
            else
            {
                PrintAppointments();
                if (GetAppointment(out appointmentId)) return;
                viewModel.AddAppointment(recommendedAppointments[appointmentId - 1], patient);
            }
            Console.WriteLine("Appointment successfully created.");
        }

        private bool GetAppointment(out int appointmentId)
        {
            while (true)
            {
                Console.Write("Choose appointment id or x for Exit: ");
                string input = Console.ReadLine();
                if (viewModel.ValidateAppointment(input, recommendedAppointments, out appointmentId)) return false;
                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Wrong input, try again.");
            }
        }

        private bool GetPriority(ref Priority? priority)
        {
            while (true)
            {
                Console.Write("Choose priority\n1. Doctor\n2. Time slot\nInput number only or x for Exit: ");
                string input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "1":
                        priority = Priority.Doctor;
                        break;
                    case "2":
                        priority = Priority.TimeSlot;
                        break;
                    case "x":
                        return true;
                    default:
                        Console.WriteLine("Wrong input, try again.");
                        break;
                }
                if (priority != null) return false;
            }
        }

        private bool GetDate(out DateTime date)
        {
            while (true)
            {
                Console.Write("Latest date(dd.MM.yyyy.) or x for Exit: ");
                string input = Console.ReadLine();
                if (viewModel.ValidateDate(input, out date))
                {
                    if (!IsDateInPast(date))
                        return false;
                    continue;
                }
                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Invalid date format!");
            }
        }

        private bool IsDateInPast(DateTime date)
        {
            if (date.Date < DateTime.Now.Date)
            {
                Console.WriteLine("The selected date cannot be in the past.");
                return true;
            }
            return false;
        }

        private bool GetLatestTime(TimeOnly earliestTime, out TimeOnly latestTime)
        {
            latestTime = new TimeOnly();
            while (true)
            {
                Console.Write("Latest time(HH:mm) or x for Exit: ");
                string input = Console.ReadLine();
                if (input.ToLower().Equals("x")) return true;
                if (ValidateLatestTime(earliestTime, ref latestTime, input)) return false;
            }
        }

        private bool ValidateLatestTime(TimeOnly earliestTime, ref TimeOnly latestTime, string input)
        {
            if (viewModel.ValidateTime(input))
            {
                latestTime = new TimeOnly(int.Parse(input.Split(":")[0]), int.Parse(input.Split(":")[1]));
                if (latestTime > earliestTime) return true;
                Console.WriteLine("Latest time is before earliest time!");
                return false;
            }
            Console.WriteLine("Please enter a valid time value in \"HH:mm\" format.");
            return false;
        }

        private bool GetEarliestTime(out TimeOnly earliestTime)
        {
            earliestTime = new TimeOnly();
            while (true)
            {
                Console.Write("Earliest time(HH:mm) or x for Exit: ");
                string input = Console.ReadLine();
                if (input.ToLower().Equals("x")) return true;
                if (!ValidateEarliestTime(ref earliestTime, input)) return false;
            }
        }

        private bool ValidateEarliestTime(ref TimeOnly earliestTime, string input)
        {
            if (viewModel.ValidateTime(input))
            {
                earliestTime = new TimeOnly(int.Parse(input.Split(":")[0]), int.Parse(input.Split(":")[1]));
                return false;
            }

            Console.WriteLine("Please enter a valid time value in \"HH:mm\" format.");
            return true;
        }

        private bool GetDoctor(out int doctorId)
        {
            while (true)
            {
                PrintDoctors();
                string input = Console.ReadLine();
                if (viewModel.ValidateDoctor(input, out doctorId)) return false;
                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Wrong input, try again.");
            }
        }

        private bool GetPatient(out int patientId)
        {
            while (true)
            {
                PrintPatients();
                string input = Console.ReadLine();
                if (viewModel.ValidatePatient(input, out patientId))
                {
                    patient = patientService.GetById(patientId);
                    return false;
                }

                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Wrong input, try again.");
            }
        }

        private void PrintPatients()
        {
            Console.WriteLine("Choose patient's Id.");
            foreach (var patient in patientService.GetPatients())
            {
                Console.WriteLine(patient);
            }
            Console.Write("x Exit\nId:");
        }

        private void PrintAppointments()
        {
            for (int i = 0; i < recommendedAppointments.Count; i++)
            {
                Console.WriteLine("Id: " + (i + 1) + ". " + recommendedAppointments[i]);
            }
        }

        private void PrintDoctors()
        {
            Console.WriteLine("Choose doctor's Id.");
            foreach (var doctor in doctorService.GetDoctors())
            {
                Console.WriteLine(doctor);
            }
            Console.Write("x Exit\nId: ");
        }
    }
}
