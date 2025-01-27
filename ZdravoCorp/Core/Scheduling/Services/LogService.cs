﻿using System;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;
using ZdravoCorp.Core.UserManager.Model;

namespace ZdravoCorp.Core.Scheduling.Services
{
    public class LogService
    {
        private ILogRepository logRepository;
        public LogService()
        {
            logRepository = Institution.Instance.LogRepository;
        }
        public void Count(int patientId)
        {
            try
            {
                foreach (var element in logRepository.GetLog().Elements)
                {
                    if (element.Appointment.PatientId == patientId)
                    {
                        if (element.Type.Equals("make"))
                        {
                            logRepository.GetLog().MakeCounter++;
                        }
                        else
                        {
                            logRepository.GetLog().UpdateCancelCounter++;
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
            if (logRepository.GetLog().MakeCounter > 8 || logRepository.GetLog().UpdateCancelCounter >= 5)
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

        public void SetLog(Log log)
        {
            logRepository.SetLog(log);
        }
    }
}
