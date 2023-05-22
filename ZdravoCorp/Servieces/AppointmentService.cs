using System;
using System.Collections.Generic;
using ZdravoCorp.Domain;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.Servieces
{
    internal class AppointmentService
    {
        public AppointmentService() { }
        public static string TakeRoom(TimeSlot timeSlot)
        {
            Dictionary<string, Room> examinationRooms = Room.LoadAllExaminationRoom();
            foreach (var room in examinationRooms)
            {
                bool check = true;
                foreach (Appointment appointment in Singleton.Instance.ScheduleRepository.Schedule.Appointments)
                {
                    if (appointment.IsCanceled) continue;
                    TimeSlotService timeSlotService = new TimeSlotService(appointment.TimeSlot);
                    if (timeSlotService.OverlapWith(timeSlot) && appointment.IdRoom == room.Key)
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
        public bool IsAbleToStart(Appointment appointment)
        {
            DateTime earliestStart = appointment.TimeSlot.start.Add(new TimeSpan(0, -15, 0));
            DateTime latestStart = appointment.TimeSlot.start.Add(new TimeSpan(0, appointment.TimeSlot.duration, 0));

            if (DateTime.Now < earliestStart || DateTime.Now > latestStart)
            {
                return false;
            }
            return true;
        }
    }
}
