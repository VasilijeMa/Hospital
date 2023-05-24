using System;
using System.Collections.Generic;

namespace ZdravoCorp.Core.Domain
{
    public class TimeSlot
    {
        public DateTime start { get; set; }
        public int duration { get; set; }
        public TimeSlot()
        {
            start = DateTime.Now;
            duration = 0;
        }
        public TimeSlot(DateTime start, int duration)
        {
            this.start = start;
            this.duration = duration;
        }
        public bool OverlapWith(TimeSlot timeSlot)
        {
            bool isAtDifferentTime = start > timeSlot.start.AddMinutes(timeSlot.duration) || start.AddMinutes(duration) < timeSlot.start;
            if (!isAtDifferentTime)
            {
                return true;
            }
            return false;
        }

        public List<TimeSlot> Split(TimeSlot timeSlot)
        {
            List<TimeSlot> timeSlots = new List<TimeSlot>();
            if (timeSlot.start > start && timeSlot.start.AddMinutes(timeSlot.duration) < start.AddMinutes(duration))
            {
                timeSlots.Add(new TimeSlot(start, (int)(timeSlot.start - start).TotalMinutes));
                timeSlots.Add(new TimeSlot(timeSlot.start.AddMinutes(timeSlot.duration), (int)(start.AddMinutes(duration) - timeSlot.start.AddMinutes(timeSlot.duration)).TotalMinutes));
            }
            else if (timeSlot.start <= start)
            {
                if (timeSlot.start.AddMinutes(timeSlot.duration) <= start.AddMinutes(duration))
                {
                    timeSlots.Add(new TimeSlot(timeSlot.start.AddMinutes(timeSlot.duration), (int)(start.AddMinutes(duration) - timeSlot.start.AddMinutes(timeSlot.duration)).TotalMinutes));
                }
                else
                {
                    timeSlots.Add(new TimeSlot(new DateTime(), 0));
                }
            }
            else if (timeSlot.start < start.AddMinutes(duration))
            {
                timeSlots.Add(new TimeSlot(start, (int)(timeSlot.start - start).TotalMinutes));
            }
            return timeSlots;
        }

    }
}
