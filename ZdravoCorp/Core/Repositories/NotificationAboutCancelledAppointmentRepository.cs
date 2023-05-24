using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories
{
    public class NotificationAboutCancelledAppointmentRepository
    {
        private List<NotificationAboutCancelledAppointment> notifications;
        public List<NotificationAboutCancelledAppointment> Notifications { get => notifications; }
        public NotificationAboutCancelledAppointmentRepository()
        {
            notifications = LoadAll();
        }
        public List<NotificationAboutCancelledAppointment> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/notifications.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<NotificationAboutCancelledAppointment>>(json);
        }

        public static void WriteAll(List<NotificationAboutCancelledAppointment> notifications)
        {
            string json = JsonConvert.SerializeObject(notifications, Formatting.Indented);
            File.WriteAllText("./../../../data/notifications.json", json);
        }

        public void Add(NotificationAboutCancelledAppointment notificationAboutCancelledAppointment)
        {
            notifications.Add(notificationAboutCancelledAppointment);
        }
    }
}
