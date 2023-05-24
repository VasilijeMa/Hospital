using System;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;

namespace ZdravoCorp.Core.Servieces
{
    public class LogService
    {
        private LogRepository logRepository;
        public LogService()
        {
            logRepository = Singleton.Instance.LogRepository;
        }
        public void Count(int patientId)
        {
            try
            {
                foreach (var element in logRepository.Log.Elements)
                {
                    if (element.Appointment.PatientId == patientId)
                    {
                        if (element.Type.Equals("make"))
                        {
                            logRepository.Log.MakeCounter++;
                        }
                        else
                        {
                            logRepository.Log.UpdateCancelCounter++;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public void CheckConditions(Patient patient)
        {
            if (logRepository.Log.MakeCounter > 8 || logRepository.Log.UpdateCancelCounter >= 5)
            {
                patient.IsBlocked = true;
            }
            else
            {
                patient.IsBlocked = false;
            }
        }

        public void AddElement(Appointment appointment, Patient patient)
        {
            logRepository.AddElement(appointment, patient);
            CheckConditions(patient);
        }
        public void UpdateCancelElement(Appointment appointment, Patient patient)
        {
            logRepository.UpdateCancelElement(appointment, patient);
            CheckConditions(patient);
        }
    }
}
