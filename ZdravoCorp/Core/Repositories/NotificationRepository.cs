using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories
{
    public class NotificationRepository
    {
        private List<Notification> notifications;
        public List<Notification> Notifications { get => notifications;}

        public NotificationRepository()
        {
            notifications = LoadAll();
        }
        public List<Notification> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/patientNotifications.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Notification>>(json);
        }
        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(notifications, Formatting.Indented);
            File.WriteAllText("./../../../data/patientNotifications.json", json);
        }
        public int getNextId()
        {
            try
            {
                return notifications.Max(notification => notification.Id) + 1;
            }
            catch
            {
                return 1;
            }
        }
        public Notification GetNotification(int notificationId)
        {
            return notifications.FirstOrDefault(notification => notification.Id == notificationId);
        }

        public void CreateNotification(string title, int patientId, int timesPerDay, int minutesBefore)
        {
            int id = getNextId();
            notifications.Add(new Notification(id, patientId, title, timesPerDay, minutesBefore));
            WriteAll();
        }

        public void UpdateNotification(int id, string title, int timesPerDay, int minutesBefore)
        {
            Notification notification = GetNotification(id);
            notification.Title = title;
            notification.TimesPerDay = timesPerDay;
            notification.MinutesBefore = minutesBefore;
            WriteAll();
        }

        public void DeleteNotification(Notification notification)
        {
            notifications.Remove(notification);
            WriteAll();
        }
    }
}
