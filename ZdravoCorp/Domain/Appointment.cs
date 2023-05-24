using System.Collections.Generic;
using System;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.Domain
{
    public class Appointment
    {
        public int Id { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public bool IsCanceled { get; set; }
        public string IdRoom { get; set; }
        public int ExaminationId{ get; set; }
        public Appointment(int id, TimeSlot timeSlot, int doctorId, int patientId, string idRoom)
        {
            Id = id;
            TimeSlot = timeSlot;
            DoctorId = doctorId;
            PatientId = patientId;
            IsCanceled = false;
            IdRoom = idRoom;
        }
        public Appointment() { }
        public static string TakeRoom(TimeSlot timeSlot)
        {
            Dictionary<string, Room> examinationRooms = Room.LoadAllExaminationRoom();
            foreach (var room in examinationRooms)
            {
                bool check = true;
                foreach (Appointment appointment in Singleton.Instance.ScheduleRepository.Schedule.Appointments)
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
        public bool IsAbleToStart()
        {
            DateTime earliestStart = TimeSlot.start.Add(new TimeSpan(0, -15, 0));
            DateTime latestStart = TimeSlot.start.Add(new TimeSpan(0, TimeSlot.duration, 0));

            if (DateTime.Now < earliestStart || DateTime.Now > latestStart)
            {
                return false;
            }
            return true;
        }

    }
}
