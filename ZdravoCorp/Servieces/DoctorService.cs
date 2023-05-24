using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Servieces
{
    public class DoctorService
    {
        private DoctorRepository doctorRepository;
        private ScheduleRepository scheduleRepository;
        public DoctorService()
        {
            this.doctorRepository = Singleton.Instance.DoctorRepository;
            this.scheduleRepository = Singleton.Instance.ScheduleRepository;
        }

        public bool IsAvailable(TimeSlot timeSlot, int doctorId, int appointmentId = -1)
        {
            foreach (Appointment appointment in scheduleRepository.GetAppointmentsForDoctor(doctorId))
            {
                if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
                if (appointment.TimeSlot.OverlapWith(timeSlot))
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
                    if (TimeSpan.Compare(appointment.TimeSlot.start.TimeOfDay, timeAfterTwoHours.TimeOfDay) == -1 && TimeSpan.Compare(appointment.TimeSlot.start.TimeOfDay, currentTime.TimeOfDay) == 1)
                    {
                        appointmentsInNextTwoHours.Add(appointment);
                    }
                }
            }
            return appointmentsInNextTwoHours;
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
                        Appointment appointment = scheduleRepository.CreateAppointment(doctorsTimeSlot,
                            qualifiedDoctor, Singleton.Instance.PatientRepository.getByUsername(patientUsername));
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
    }
}
