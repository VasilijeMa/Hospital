using System.Collections.Generic;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Servieces
{
    public class PatientService
    {
        PatientRepository patientRepository;
        public PatientService()
        {
            patientRepository = new PatientRepository();
        }
        public static Patient getById(int id)
        {
            foreach (Patient patient in PatientRepository.patients)
            {
                if (patient.Id == id)
                {
                    return patient;
                }
            }
            return null;
        }
        public static Patient getByUsername(string username)
        {
            foreach (Patient patient in PatientRepository.patients)
            {
                if (patient.Username == username)
                {
                    return patient;
                }
            }
            return null;
        }
        public bool IsAvailable(TimeSlot timeSlot, int patientId, int appointmentId = -1)
        {
            foreach (Appointment appointment in Singleton.Instance.Schedule.Appointments)
            {
                if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
                TimeSlotService timeSlotService = new TimeSlotService(appointment.TimeSlot);
                if (patientId == appointment.PatientId && timeSlotService.OverlapWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public void WriteAll(List<Patient> patients)
        {
            patientRepository.WriteAll(patients);
        }
    }
}
