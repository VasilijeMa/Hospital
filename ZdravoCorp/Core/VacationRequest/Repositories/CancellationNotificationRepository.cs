using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Repositories.Interfaces;

namespace ZdravoCorp.Core.VacationRequest.Repositories
{
    public class CancellationNotificationRepository : ICancellationNotificationRepository
    {
        private List<CancellationNotification> _cancellationNotifications;
        public List<CancellationNotification> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/cancellationNotifications.json");
            var json = reader.ReadToEnd();
            List<CancellationNotification> allNotifications = JsonConvert.DeserializeObject<List<CancellationNotification>>(json);

            return allNotifications;
        }

        public CancellationNotificationRepository()
        {
            _cancellationNotifications = LoadAll();
        }
        public void SaveAll()
        {
            string json = JsonConvert.SerializeObject(_cancellationNotifications, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/cancellationNotifications.json", json);
        }
        public void AddAll(List<CancellationNotification> notifications)
        {
            foreach(CancellationNotification notification in notifications)
            {
                _cancellationNotifications.Add(notification);
            }
            SaveAll();
        }
        public List<Appointment> ExecuteNotifications(int patientId)
        {
            List<Appointment> cancelledAppointments = new List<Appointment> ();
            foreach (CancellationNotification notification in _cancellationNotifications)
            {
                if (!notification.IsNotificationExecutable(patientId)) continue;
                notification.IsUserNotified = true;
                cancelledAppointments.Add(notification.CancelledAppointment);
            }
            SaveAll();
            return cancelledAppointments;
        }


    }
}
