using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.Core.Servieces
{
    public class ScheduleService
    {
        private IScheduleRepository scheduleRepository;

        public ScheduleService()
        {
            scheduleRepository = Singleton.Instance.ScheduleRepository;
        }

        public List<Appointment> GetAllAppointmentsForDoctor(DateTime startDate, DateTime endDate, int doctorId)
        {
            List<Appointment> doctorsAppointments = new List<Appointment>();

            while (startDate <= endDate)
            {
                foreach (Appointment appointment in scheduleRepository.GetAppointmentsForDoctor(doctorId))
                {
                    if (appointment.TimeSlot.start.Date == startDate)
                    {
                        doctorsAppointments.Add(appointment);
                    }
                }
                startDate = startDate.AddDays(1);
            }
            return doctorsAppointments;
        }

        public Appointment UpdateAppointment(int appointmentId, TimeSlot timeSlot, int doctorId, Patient patient = null)
        {
            return scheduleRepository.UpdateAppointment(appointmentId, timeSlot, doctorId, patient);
        }

        public Appointment CreateAppointment(TimeSlot timeSlot, Doctor doctor, Patient patient, int idExamination = 0)
        {
            string roomId = TakeRoom(timeSlot);
            if (roomId == "")
            {
                MessageBox.Show("All rooms are full.");
                return null;
            }
            return scheduleRepository.CreateAppointment(timeSlot, doctor.Id, patient.Id, roomId, idExamination);
        }

        public string TakeRoom(TimeSlot timeSlot)
        {
            Dictionary<string, Room> examinationRooms = Room.LoadAllExaminationRoom();
            foreach (var room in examinationRooms)
            {
                bool check = true;
                foreach (Appointment appointment in scheduleRepository.GetAppointments())
                {
                    if (appointment.IsCanceled) continue;
                    if (appointment.TimeSlot.OverlapWith(timeSlot) && appointment.IdRoom == room.Key)
                    {
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    return room.Key;
                }
            }
            return "";
        }

        public Appointment CreateAppointment(Appointment appointment)
        {
            return scheduleRepository.CreateAppointment(appointment);
        }

        public void WriteAllAppointmens()
        {
            scheduleRepository.WriteAllAppointmens();
        }

        public Appointment GetAppointmentById(int id)
        {
            return scheduleRepository.GetAppointmentById(id);
        }

        public List<Appointment> GetAppointmentsForPatient(int patientId)
        {
            return scheduleRepository.GetAppointmentsForPatient(patientId);
        }

        public Appointment CancelAppointment(int appointmentId)
        {
            return scheduleRepository.CancelAppointment(appointmentId);
        }

        public List<Appointment> GetTodaysAppointments()
        {
            return scheduleRepository.GetTodaysAppointments();
        }
    }
}
