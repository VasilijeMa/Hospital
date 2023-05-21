using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Repositories
{
    public class NotificarionAboutCancelledAppointmentRepository
    {
        public static List<NotificationAboutCancelledAppointment> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/notifications.json");
            var json = reader.ReadToEnd();
            List<NotificationAboutCancelledAppointment> notifications = JsonConvert.DeserializeObject<List<NotificationAboutCancelledAppointment>>(json);
            return notifications;
        }

        public static void WriteAll(List<NotificationAboutCancelledAppointment> notifications)
        {
            string json = JsonConvert.SerializeObject(notifications, Formatting.Indented);
            File.WriteAllText("./../../../data/notifications.json", json);
        }
    }
}
