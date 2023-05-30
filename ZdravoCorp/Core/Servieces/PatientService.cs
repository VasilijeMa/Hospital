using System.Collections.Generic;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class PatientService
    {
        IPatientRepository patientRepository;
        IScheduleRepository scheduleRepository;
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

        public Patient GetById(int id)
        {
            return patientRepository.getById(id);
        }

        public List<Patient> GetPatients()
        {
            return patientRepository.GetPatients();
        }

        public void AddPatient(Patient patient)
        {
            patientRepository.AddPatient(patient);
        }

        public void RemovePatient(Patient patient)
        {
            patientRepository.RemovePatient(patient);
        }

        public Patient GetByUsername(string username)
        {
            return patientRepository.getByUsername(username);
        }
    }
}
