using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class TimeSlot
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
            if(this.start.Date == timeSlot.start.Date)
            {
                bool isAtDifferentTime = start.TimeOfDay > timeSlot.start.AddMinutes(timeSlot.duration).TimeOfDay || start.AddMinutes(duration).TimeOfDay < timeSlot.start.TimeOfDay;
                if (isAtDifferentTime)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
