using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Servieces
{
    public class TimeSlotService
    {
        TimeSlot TimeSlot { get; set; }
        public TimeSlotService(TimeSlot timeSlot)
        {
            this.TimeSlot = timeSlot;
        }
        public bool OverlapWith(TimeSlot timeSlot)
        {
            bool isAtDifferentTime = TimeSlot.start > timeSlot.start.AddMinutes(timeSlot.duration) || TimeSlot.start.AddMinutes(TimeSlot.duration) < timeSlot.start;
            if (!isAtDifferentTime)
            {
                return true;
            }
            return false;
        }

        public List<TimeSlot> Split(TimeSlot timeSlot)
        {
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            if (timeSlot.start > TimeSlot.start && timeSlot.start.AddMinutes(timeSlot.duration) < TimeSlot.start.AddMinutes(TimeSlot.duration))
            {
                timeSlots.Add(new TimeSlot(TimeSlot.start, (int)(timeSlot.start - TimeSlot.start).TotalMinutes));
                timeSlots.Add(new TimeSlot(timeSlot.start.AddMinutes(timeSlot.duration), (int)(TimeSlot.start.AddMinutes(TimeSlot.duration) - timeSlot.start.AddMinutes(timeSlot.duration)).TotalMinutes));
            }
            else if (timeSlot.start <= TimeSlot.start)
            {
                if (timeSlot.start.AddMinutes(timeSlot.duration) <= TimeSlot.start.AddMinutes(TimeSlot.duration))
                {
                    timeSlots.Add(new TimeSlot(timeSlot.start.AddMinutes(timeSlot.duration), (int)(TimeSlot.start.AddMinutes(TimeSlot.duration) - timeSlot.start.AddMinutes(timeSlot.duration)).TotalMinutes));
                }
                else
                {
                    timeSlots.Add(new TimeSlot(new DateTime(), 0));
                }
            }
            else if (timeSlot.start < TimeSlot.start.AddMinutes(TimeSlot.duration))
            {
                timeSlots.Add(new TimeSlot(TimeSlot.start, (int)(timeSlot.start - TimeSlot.start).TotalMinutes));
            }
            return timeSlots;
        }
    }
}
