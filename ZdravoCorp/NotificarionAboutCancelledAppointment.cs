using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class NotificationAboutCancelledAppointment
    {
        public int AppointmenntId { get; set; }
        public int DoctorId { get; set; }
        public bool isShown { get; set; }
        public NotificationAboutCancelledAppointment(int appointmenntId, int doctorId,bool isshown)
        {
            AppointmenntId = appointmenntId;
            DoctorId = doctorId;
            isShown = isshown;
        }

        public static List<NotificationAboutCancelledAppointment> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/notifications.json");
            var json = reader.ReadToEnd();
            List<NotificationAboutCancelledAppointment> notifications = JsonConvert.DeserializeObject<List<NotificationAboutCancelledAppointment>>(json);
            return notifications;
        }

        public void WriteAll(List<NotificationAboutCancelledAppointment> notifications)
        {
            string json = JsonConvert.SerializeObject(notifications, Formatting.Indented);
            File.WriteAllText("./../../../data/notifications.json", json);
        }
    }
}
