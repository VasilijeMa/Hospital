using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class ScheduleService
    {
        private IScheduleRepository scheduleRepository;
        private IRoomRepository roomRepository;

        public ScheduleService()
        {
            scheduleRepository = Singleton.Instance.ScheduleRepository;
            roomRepository = new RoomRepository();
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
            Dictionary<string, Room> examinationRooms = roomRepository.LoadAllExaminationRooms();
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

        public Appointment GetAppointmentByExaminationId(int examinationId) 
        {
            foreach (Appointment appointment in scheduleRepository.GetAppointments()) 
            {
                if (appointment.ExaminationId == examinationId)
                {
                    return appointment;
                }
            }
            return null;
        }

        public Appointment CreateAppointment(Appointment appointment)
        {
            string roomId = TakeRoom(appointment.TimeSlot);
            appointment.IdRoom = roomId;
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
