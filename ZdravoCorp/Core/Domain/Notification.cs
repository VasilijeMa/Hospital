using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TimesPerDay { get; set; }
        public int MinutesBefore { get; set; }
        public Notification(){}
        public Notification(int id, string title, int timesPerDay, int minutesBefore)
        {
            Id = id;
            Title = title;
            TimesPerDay = timesPerDay;
            MinutesBefore = minutesBefore;
        }
    }
}
