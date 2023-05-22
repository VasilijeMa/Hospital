using ZdravoCorp.Domain;

namespace ZdravoCorp.Servieces
{
    public class LogService
    {
        public static void Count(int patientId)
        {
            try
            {
                foreach (var element in Singleton.Instance.LogRepository.Log.Elements)
                {
                    if (element.Appointment.PatientId == patientId)
                    {
                        if (element.Type.Equals("make"))
                        {
                            Singleton.Instance.LogRepository.Log.MakeCounter++;
                        }
                        else
                        {
                            Singleton.Instance.LogRepository.Log.UpdateCancelCounter++;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public static void CheckConditions(Patient patient)
        {
            if (Singleton.Instance.LogRepository.Log.MakeCounter > 8 || Singleton.Instance.LogRepository.Log.UpdateCancelCounter >= 5)
            {
                patient.IsBlocked = true;
            }
            else
            {
                patient.IsBlocked = false;
            }
        }
    }
}
