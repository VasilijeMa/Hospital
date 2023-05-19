﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Domain
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
    }
}
