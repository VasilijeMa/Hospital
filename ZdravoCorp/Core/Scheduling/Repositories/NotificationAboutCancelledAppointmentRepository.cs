using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;

namespace ZdravoCorp.Core.Scheduling.Repositories
{
    public class NotificationAboutCancelledAppointmentRepository : INotificationAboutCancelledAppointmentRepository
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

        public void WriteAll(List<NotificationAboutCancelledAppointment> notifications)
        {
            string json = JsonConvert.SerializeObject(notifications, Formatting.Indented);
            File.WriteAllText("./../../../data/notifications.json", json);
        }

        public void Add(NotificationAboutCancelledAppointment notificationAboutCancelledAppointment)
        {
            notifications.Add(notificationAboutCancelledAppointment);
        }

        public List<NotificationAboutCancelledAppointment> GetNotifications()
        {
            return Notifications;
        }
    }
}
