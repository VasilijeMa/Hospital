namespace ZdravoCorp.Core.Domain
{
    public class NotificationAboutCancelledAppointment
    {
        public int AppointmenntId { get; set; }
        public int DoctorId { get; set; }
        public bool isShown { get; set; }
        public NotificationAboutCancelledAppointment(int appointmenntId, int doctorId, bool isshown)
        {
            AppointmenntId = appointmenntId;
            DoctorId = doctorId;
            isShown = isshown;
        }
    }
}
