using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Domain;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.Servieces
{
    internal class AppointmentService
    {
        Appointment Appointment { get; set; }
        public AppointmentService(Appointment appointment)
        {
            Appointment = appointment;
        }
        public static string TakeRoom(TimeSlot timeSlot)
        {
            Dictionary<string, Room> examinationRooms = Room.LoadAllExaminationRoom();
            foreach (var room in examinationRooms)
            {
                bool check = true;
                foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
                {
                    if (appointment.IsCanceled) continue;
                    TimeSlotService timeSlotService = new TimeSlotService(timeSlot);
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
        public bool IsAbleToStart()
        {
            DateTime earliestStart = Appointment.TimeSlot.start.Add(new TimeSpan(0, -15, 0));
            DateTime latestStart = Appointment.TimeSlot.start.Add(new TimeSpan(0, Appointment.TimeSlot.duration, 0));

            if (DateTime.Now < earliestStart || DateTime.Now > latestStart)
            {
                return false;
            }
            return true;
        }
    }
}
