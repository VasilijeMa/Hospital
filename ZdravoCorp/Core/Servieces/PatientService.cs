using System;
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
        IExaminationRepository examinationRepository;

        public PatientService()
        {
            patientRepository = Singleton.Instance.PatientRepository;
            scheduleRepository = Singleton.Instance.ScheduleRepository;
            examinationRepository = Singleton.Instance.ExaminationRepository;
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

        public List<Patient> AlreadyExaminedPatients(int doctorId)
        {
            List<Patient> patients = new List<Patient>();
            foreach (Appointment appointment in scheduleRepository.GetAppointmentsForDoctor(doctorId))
            {
                if (appointment.IsCanceled) continue;
                if (appointment.TimeSlot.start < DateTime.Now)
                {
                    Patient patient = GetById(appointment.PatientId);
                    if (!patients.Contains(patient)) patients.Add(patient);
                }
            }
            return patients;
        }

        public List<Patient> CurrentlyHospitalizedPatients(int doctorId)
        {
            List<Patient> patients = new List<Patient>();
            foreach (var examination in examinationRepository.GetExaminations())
            {
                Appointment appointment = scheduleRepository.GetAppointmentByExaminationId(examination.Id);
                Patient hospitalizedPatient = GetById(appointment.PatientId);
                if (AlreadyExaminedPatients(doctorId).Contains(hospitalizedPatient))
                {
                    if (!patients.Contains(hospitalizedPatient)) patients.Add(hospitalizedPatient);
                }
            }
            return patients;
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
