using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Model;

namespace ZdravoCorp.Core.VacationRequest.Model
{
    public class CancellationNotification
    {
        public Appointment CancelledAppointment { get; set; }
        public bool IsUserNotified { get; set; }
        public CancellationNotification(Appointment appointment)
        {
            CancelledAppointment = appointment;
            IsUserNotified = false;
        }
        public bool IsNotificationExecutable(int patientId)
        {
            return !IsUserNotified && CancelledAppointment.PatientId == patientId;
        }
    }
}
