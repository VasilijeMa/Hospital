using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Domain.Enums;
using ZdravoCorp.Servieces;

namespace ZdravoCorp.Domain
{
    public class Doctor : User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Specialization Specialization { get; set; }

        public Doctor() : base() { }

        public Doctor(int id, string name, string lastname, Specialization specialization, string username, string password, string type) : base(username, password, type)
        {
            Id = id;
            FirstName = name;
            LastName = lastname;
            Specialization = specialization;
        }

        public List<Appointment> GetAllAppointments(DateTime startDate, DateTime endDate)
        {
            List<Appointment> appointments = new List<Appointment>();

            while (startDate <= endDate)
            {
                foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
                {
                    if (appointment.TimeSlot.start.Date == startDate && appointment.DoctorId == Id)
                    {
                        appointments.Add(appointment);
                    }
                }
                startDate = startDate.AddDays(1);
            }
            return appointments;
        }

        public List<Appointment> GetAppointmentsInNextTwoHours()
        {
            DateTime currentTime = DateTime.Now;
            DateTime timeAfterTwoHours = DateTime.Now.AddHours(2);
            List<Appointment> appointmentsInNextTwoHours = new List<Appointment>();

            foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
            {
                if (appointment.TimeSlot.start.Date == timeAfterTwoHours.Date)
                {
                    if (TimeSpan.Compare(appointment.TimeSlot.start.TimeOfDay, timeAfterTwoHours.TimeOfDay) == -1 && TimeSpan.Compare(appointment.TimeSlot.start.TimeOfDay, currentTime.TimeOfDay) == 1 && appointment.DoctorId == Id)
                    {
                        appointmentsInNextTwoHours.Add(appointment);
                    }
                }
            }
            return appointmentsInNextTwoHours;
        }

        public bool IsAvailable(TimeSlot timeSlot, int appointmentId = -1)
        {
            foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
            {
                if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
                TimeSlotService timeSlotService = new TimeSlotService(timeSlot);
                if (Id == appointment.DoctorId && timeSlotService.OverlapWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public static List<Doctor> GetDoctorBySpecialization(string specialization)
        {
            return Singleton.Instance.doctors.Where(doctor => doctor.Specialization.ToString() == specialization).ToList();
        }

        public bool IsAlreadyExamined(int id)
        {
            foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
            {
                if (appointment.IsCanceled) continue;
                if (appointment.PatientId == id && appointment.DoctorId == Id)
                {
                    if (appointment.TimeSlot.start < DateTime.Now)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
