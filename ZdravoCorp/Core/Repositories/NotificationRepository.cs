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

    }
}
