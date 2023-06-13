using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorpCLI
{
    public class RecommendingAppointmentsCLIView
    {
        Patient patient;
        List<Appointment> recommendedAppointments;
        private DoctorService doctorService = new DoctorService();
        private PatientService patientService = new PatientService();
        private ScheduleService scheduleService = new ScheduleService();
        private LogService logService = new LogService();
        private RecommendingAppointmentsService recommendingAppointmentsService = new RecommendingAppointmentsService();
        private RecommendingAppointmentsCLIViewModel viewModel = new RecommendingAppointmentsCLIViewModel();
        public RecommendingAppointmentsCLIView()
        {
            int patientId;
            while (true)
            {
                PrintPatients();
                string input = Console.ReadLine();
                if (viewModel.ValidatePatient(input, out patientId))
                {
                    patient = patientService.GetById(patientId);
                    break;
                }
                if (input.ToLower().Equals("x")) return;
                Console.WriteLine("Wrong input, try again.");
            }

            int doctorId;
            while (true)
            {
                PrintDoctors();
                string input = Console.ReadLine();
                if (viewModel.ValidateDoctor(input, out doctorId)) break;
                if (input.ToLower().Equals("x")) return;
                Console.WriteLine("Wrong input, try again.");
            }

            TimeOnly earliestTime;
            while (true)
            {
                Console.Write("Earliest time(HH:mm) or x for Exit: ");
                string input = Console.ReadLine();
                if (viewModel.ValidateTime(input))
                {
                    earliestTime = new TimeOnly(int.Parse(input.Split(":")[0]), int.Parse(input.Split(":")[1]));
                    break;
                }
                if (input.ToLower().Equals("x")) return;
                Console.WriteLine("Please enter a valid time value in \"HH:mm\" format.");
            }

            TimeOnly latestTime;
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
                if (input.ToLower().Equals("x")) return;
                Console.WriteLine("Please enter a valid time value in \"HH:mm\" format.");
            }

            DateTime date;
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
                if (input.ToLower().Equals("x")) return;
                Console.WriteLine("Invalid date format!");
            }

            Priority? priority = null;
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
                        return;
                    default:
                        Console.WriteLine("Wrong input, try again.");
                        break;
                }
                if (priority != null) break;
            }


            AppointmentRequest appointmentRequest = new AppointmentRequest(doctorService.GetDoctor(doctorId),
                earliestTime, latestTime, date, (Priority)priority);
            List<Appointment> recommendedAppointments =
                recommendingAppointmentsService.GetAppointmentsByRequest(appointmentRequest, patientId);
            int appointmentId;
            if (recommendedAppointments.Count() == 1)
            {
                scheduleService.CreateAppointment(recommendedAppointments[0]);
                logService.AddElement(recommendedAppointments[0], patient);
                Console.WriteLine("Appointment " + recommendedAppointments[0]);
                Console.WriteLine("Appointment successfully created.");
            }
            else
            {
                PrintAppointments(recommendedAppointments);
                while (true)
                {
                    Console.Write("Choose appointment id or x for Exit: ");
                    string input = Console.ReadLine();
                    if (Int32.TryParse(input, out appointmentId))
                    {
                        try
                        {
                            Appointment a = recommendedAppointments[appointmentId - 1];
                            break;
                        }
                        catch
                        {
                            Console.WriteLine("Wrong id, try again.");
                            continue;
                        }
                    }
                    if (input.ToLower().Equals("x")) return;
                    Console.WriteLine("Wrong input, try again.");
                }
                scheduleService.CreateAppointment(recommendedAppointments[appointmentId - 1]);
                logService.AddElement(recommendedAppointments[appointmentId - 1], patient);
                Console.WriteLine("Appointment successfully created.");
            }
            scheduleService.WriteAllAppointmens();
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
