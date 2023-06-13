using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;

namespace ZdravoCorpCLI
{
    public class RecommendingAppointmentsCLIViewModel
    {
        private PatientService patientService = new PatientService();
        private DoctorService doctorService = new DoctorService();
        private ScheduleService scheduleService = new ScheduleService();
        private LogService logService = new LogService();
        private RecommendingAppointmentsService recommendingAppointmentsService = new RecommendingAppointmentsService();


        public bool ValidatePatient(string? input, out int patientId)
        {
            if (Int32.TryParse(input, out patientId) && patientService.GetById(patientId) != null)
            {
                return true;
            }
            return false;
        }

        public bool ValidateDoctor(string? input, out int doctorId)
        {
            if (Int32.TryParse(input, out doctorId) && doctorService.GetDoctor(doctorId) != null)
            {
                return true;
            }
            return false;
        }

        public bool ValidateTime(string? input)
        {
            string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
            if (System.Text.RegularExpressions.Regex.IsMatch(input, pattern))
            {
                return true;
            }
            return false;
        }

        public bool ValidateDate(string? input, out DateTime date)
        {
            string[] formats = { "dd.MM.yyyy." };
            bool isValid = DateTime.TryParseExact(input, formats, null, System.Globalization.DateTimeStyles.None, out date);
            return isValid;
        }

        public void AddAppointment(Appointment appointment, Patient patient)
        {
            scheduleService.CreateAppointment(appointment);
            logService.AddElement(appointment, patient);
            scheduleService.WriteAllAppointmens();
        }

        public List<Appointment> FoundAppointments(AppointmentRequest appointmentRequest, int patientId)
        {
            return recommendingAppointmentsService.GetAppointmentsByRequest(appointmentRequest, patientId);
        }

        public bool ValidateAppointment(string? input, List<Appointment> recommendedAppointments, out int appointmentId)
        {
            if (Int32.TryParse(input, out appointmentId))
            {
                try
                {
                    Appointment a = recommendedAppointments[appointmentId - 1];
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
