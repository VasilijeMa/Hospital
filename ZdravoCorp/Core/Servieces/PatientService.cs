using System.Collections.Generic;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;

namespace ZdravoCorp.Core.Servieces
{
    public class PatientService
    {
        PatientRepository patientRepository;
        ScheduleRepository scheduleRepository;
        public PatientService()
        {
            patientRepository = Singleton.Instance.PatientRepository;
            scheduleRepository = Singleton.Instance.ScheduleRepository;
        }
        public bool IsAvailable(TimeSlot timeSlot, int patientId, int appointmentId = -1)
        {
            foreach (Appointment appointment in scheduleRepository.GetAppointmentsForPatient(patientId))
            {
                if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
                if (appointment.TimeSlot.OverlapWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public void WriteAll()
        {
            patientRepository.WriteAll();
        }
    }
}
