using System;
using System.Collections.Generic;
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
            int patientId;
            if (GetPatient(out patientId)) return;

            int doctorId;
            if (GetDoctor(out doctorId)) return;

            TimeOnly earliestTime;
            if (GetEarliestTime(out earliestTime)) return;

            TimeOnly latestTime;
            if (GetLatestTime(earliestTime, out latestTime)) return;

            DateTime date;
            if (GetDate(out date)) return;

            Priority? priority = null;
            if (GetPriority(ref priority)) return;

            AppointmentRequest appointmentRequest = new AppointmentRequest(doctorService.GetDoctor(doctorId),
                earliestTime, latestTime, date, (Priority)priority);
            List<Appointment> recommendedAppointments =
                viewModel.FoundAppointments(appointmentRequest, patientId);
            int appointmentId;
            if (recommendedAppointments.Count() == 1)
            {
                viewModel.AddAppointment(recommendedAppointments[0], patient);
                Console.WriteLine("Appointment " + recommendedAppointments[0]);
            }
            else
            {
                PrintAppointments(recommendedAppointments);
                if (GetAppointment(recommendedAppointments, out appointmentId)) return;
                viewModel.AddAppointment(recommendedAppointments[appointmentId - 1], patient);
            }
            Console.WriteLine("Appointment successfully created.");
        }

        private bool GetAppointment(List<Appointment> recommendedAppointments, out int appointmentId)
        {
            while (true)
            {
                Console.Write("Choose appointment id or x for Exit: ");
                string input = Console.ReadLine();
                if (viewModel.ValidateAppointment(input, recommendedAppointments, out appointmentId)) break;
                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Wrong input, try again.");
            }

            return false;
        }

        private static bool GetPriority(ref Priority? priority)
        {
            while (true)
            {
                Console.WriteLine("Choose priority");
                Console.WriteLine("1. Doctor");
                Console.WriteLine("2. Time slot");
                Console.WriteLine("Input number only or x for Exit");
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
                if (priority != null) break;
            }
            return false;
        }

        private bool GetDate(out DateTime date)
        {
            while (true)
            {
                Console.Write("Latest date(dd.MM.yyyy.) or x for Exit: ");
                string input = Console.ReadLine();
                if (viewModel.ValidateDate(input, out date))
                {
                    if (date.Date < DateTime.Now.Date)
                    {
                        Console.WriteLine("The selected date cannot be in the past.");
                        continue;
                    }
                    break;
                }
                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Invalid date format!");
            }
            return false;
        }

        private bool GetLatestTime(TimeOnly earliestTime, out TimeOnly latestTime)
        {
            while (true)
            {
                Console.Write("Latest time(HH:mm) or x for Exit: ");
                string input = Console.ReadLine();
                if (viewModel.ValidateTime(input))
                {
                    latestTime = new TimeOnly(int.Parse(input.Split(":")[0]), int.Parse(input.Split(":")[1]));
                    if (latestTime > earliestTime) break;
                    Console.WriteLine("Latest time is before earliest time!");
                    continue;
                }
                latestTime = new TimeOnly();
                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Please enter a valid time value in \"HH:mm\" format.");
            }

            return false;
        }

        private bool GetEarliestTime(out TimeOnly earliestTime)
        {
            while (true)
            {
                Console.Write("Earliest time(HH:mm) or x for Exit: ");
                string input = Console.ReadLine();
                if (viewModel.ValidateTime(input))
                {
                    earliestTime = new TimeOnly(int.Parse(input.Split(":")[0]), int.Parse(input.Split(":")[1]));
                    break;
                }
                earliestTime = new TimeOnly();
                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Please enter a valid time value in \"HH:mm\" format.");
            }

            return false;
        }

        private bool GetDoctor(out int doctorId)
        {
            while (true)
            {
                PrintDoctors();
                string input = Console.ReadLine();
                if (viewModel.ValidateDoctor(input, out doctorId)) break;
                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Wrong input, try again.");
            }
            return false;
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
                    break;
                }

                if (input.ToLower().Equals("x")) return true;
                Console.WriteLine("Wrong input, try again.");
            }

            return false;
        }

        private void PrintPatients()
        {
            Console.WriteLine("Choose patient's Id.");
            foreach (var patient in patientService.GetPatients())
            {
                Console.WriteLine(patient);
            }
            Console.WriteLine("x Exit");
            Console.Write("Id:");
        }

        public void PrintAppointments(List<Appointment> appointments)
        {
            for (int i = 0; i < appointments.Count; i++)
            {
                Console.WriteLine("Id: " + (i + 1) + ". " + appointments[i]);
            }
        }

        public void PrintDoctors()
        {
            Console.WriteLine("Choose doctor's Id.");
            List<Doctor> doctors = doctorService.GetDoctors();
            foreach (var doctor in doctors)
            {
                Console.WriteLine(doctor);
            }
            Console.WriteLine("x Exit");
            Console.Write("Id:");
        }
    }
}
