using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorpCLI
{
    public class RecommendingAppointmentsCLIViewModel
    {
        private PatientService patientService = new PatientService();
        private DoctorService doctorService = new DoctorService();

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
    }
}
