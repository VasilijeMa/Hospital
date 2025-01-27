﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Repositories.Interfaces;

namespace ZdravoCorp.Core.UserManager.Services
{
    public class DoctorService
    {
        private IDoctorRepository doctorRepository;
        private IScheduleRepository scheduleRepository;
        private IPatientRepository patientRepository;
        private ScheduleService scheduleService = new ScheduleService();
        List<DoctorAvailability> doctorAvailabilities;

        public DoctorService()
        {
            doctorRepository = Institution.Instance.DoctorRepository;
            scheduleRepository = Institution.Instance.ScheduleRepository;
            patientRepository = Institution.Instance.PatientRepository;
            doctorAvailabilities = new List<DoctorAvailability>();
            doctorAvailabilities.Add(new DoctorAvailabilityByAppointment());
            doctorAvailabilities.Add(new DoctorAvailabilityByVacation());
        }

        //public bool IsAvailable(TimeSlot timeSlot, int doctorId, int appointmentId = -1)
        //{
        //    foreach (Appointment appointment in scheduleRepository.GetAppointmentsForDoctor(doctorId))
        //    {
        //        if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
        //        if (appointment.TimeSlot.OverlapWith(timeSlot))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        public bool IsAvailable(TimeSlot timeSlot, int doctorId, int appointmentId = -1)
        {
            foreach (var doctorAvailability in doctorAvailabilities)
            {
                if (!doctorAvailability.IsAvailable(timeSlot, doctorId, appointmentId))
                {
                    return false;
                }
            }
            return true;
        }
        
        public bool IsAlreadyExamined(int patientId, int doctorId)
        {
            foreach (Appointment appointment in scheduleRepository.GetAppointmentsForPatientAndDoctor(patientId, doctorId))
            {
                if (appointment.IsCanceled) continue;
                if (appointment.TimeSlot.start < DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Appointment> GetAppointmentsInNextTwoHours(int doctorId)
        {
            DateTime currentTime = DateTime.Now;
            DateTime timeAfterTwoHours = DateTime.Now.AddHours(2);
            List<Appointment> appointmentsInNextTwoHours = new List<Appointment>();

            foreach (Appointment appointment in scheduleRepository.GetAppointmentsForDoctor(doctorId))
            {
                if (appointment.TimeSlot.start.Date == timeAfterTwoHours.Date)
                {
                    if (IsAppointmentInNextTwoHours(appointment, timeAfterTwoHours, currentTime))
                    {
                        appointmentsInNextTwoHours.Add(appointment);
                    }
                }
            }
            return appointmentsInNextTwoHours;
        }

        private static bool IsAppointmentInNextTwoHours(Appointment appointment, DateTime timeAfterTwoHours, DateTime currentTime)
        {
            return TimeSpan.Compare(appointment.TimeSlot.start.TimeOfDay, timeAfterTwoHours.TimeOfDay) == -1 && TimeSpan.Compare(appointment.TimeSlot.start.TimeOfDay, currentTime.TimeOfDay) == 1;
        }

        public List<Doctor> GetDoctorBySpecialization(string specialization)
        {
            return doctorRepository.GetDoctorBySpecialization(specialization);
        }

        public List<Appointment> getAppointmentsInNextTwoHours(List<Doctor> qualifiedDoctors)
        {
            List<Appointment> allAppointments = new List<Appointment>();
            foreach (Doctor doctor in qualifiedDoctors)
            {
                List<Appointment> appointmentsForOne = GetAppointmentsInNextTwoHours(doctor.Id);
                if (appointmentsForOne.Count() == 0) { continue; }
                allAppointments.AddRange(appointmentsForOne);
            }
            return allAppointments;
        }

        public Doctor GetSpecializedDoctor(string specialization, DateTime firstDate, DateTime secondDate)
        {
            List<Doctor> doctors = GetDoctorBySpecialization(specialization);
            int minimalNumberOfAppointments = 100;
            Doctor selectedDoctor = null;
            foreach (Doctor doctor in doctors)
            {
                List<Appointment> appointments = scheduleService.GetAllAppointmentsForDoctor(firstDate, secondDate, doctor.Id);
                if (minimalNumberOfAppointments > appointments.Count())
                {
                    minimalNumberOfAppointments = appointments.Count();
                    selectedDoctor = doctor;
                }
            }
            return selectedDoctor;
        }
        public Doctor getFirstFreeDoctor(List<Doctor> qualifiedDoctors, int duration, string patientUsername)
        {
            DateTime currentTime = DateTime.Now;
            DateTime timeAfterTwoHours = DateTime.Now.AddHours(2);
            foreach (Doctor qualifiedDoctor in qualifiedDoctors)
            {
                for (DateTime time = currentTime; time < timeAfterTwoHours; time = time.AddMinutes(5))
                {
                    TimeSlot doctorsTimeSlot = new TimeSlot(currentTime, duration);
                    if (IsAvailable(doctorsTimeSlot, qualifiedDoctor.Id))
                    {

                        string roomId = scheduleService.TakeRoom(doctorsTimeSlot);
                        //TakeRoom
                        Appointment appointment = scheduleRepository.CreateAppointment(doctorsTimeSlot,
                            qualifiedDoctor.Id, patientRepository.getByUsername(patientUsername).Id);
                        if (appointment != null)
                        {
                            scheduleRepository.WriteAllAppointmens();
                        }
                        return qualifiedDoctor;
                    }
                }
            }
            return null;
        }

        public List<Doctor> GetDoctors()
        {
            return doctorRepository.GetDoctors();
        }

        public List<Specialization> GetSpecializationOfDoctors()
        {
            return doctorRepository.GetSpecializationOfDoctors();
        }

        public Doctor GetDoctor(int doctorId)
        {
            return doctorRepository.getDoctor(doctorId);
        }

        public Doctor GetByUsername(string username)
        {
            return doctorRepository.GetByUsername(username);
        }

        public List<Doctor> SearchDoctors(string keyword)
        {
            return doctorRepository.SearchDoctors(keyword);
        }

        public void AddRating(int doctorId, int serviceQuality, int suggestToFriends)
        {
            doctorRepository.AddRating(doctorId, serviceQuality, suggestToFriends);
        }
    }
}
