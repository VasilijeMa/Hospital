using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class FreeDays
    {
        public int DoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public string Reason { get; set; }

        public FreeDays(int id, DateTime startDate, int duration, string reason)
        {
            DoctorId = id;
            StartDate = startDate;
            Duration = duration;
            Reason = reason;
        }

        public FreeDays() { }
    }
}
