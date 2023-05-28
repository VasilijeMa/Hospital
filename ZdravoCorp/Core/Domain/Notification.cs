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
        public int PatientId { get; set; }
        public string Message { get; set; }
        public int TimesPerDay { get; set; }
        public int MinutesBefore { get; set; }
        public DateTime? Date { get; set; }
        public bool IsActive { get; set; }
        public Notification(){}
        public Notification(int id, int patientId, string message, int timesPerDay, int minutesBefore, DateTime? date)
        {
            Id = id;
            PatientId = patientId;
            Message = message;
            TimesPerDay = timesPerDay;
            MinutesBefore = minutesBefore;
            Date = date;
            IsActive = true;
        }
    }
}
