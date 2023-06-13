using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientNotification.Model;

namespace ZdravoCorp.Core.PatientNotification.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        public List<Notification> LoadAll();

        public void WriteAll();

        public Notification GetNotification(int notificationId);

        public void CreateNotification(string message, int patientId, int timesPerDay, int minutesBefore,
            DateTime? date);

        public void UpdateNotification(int id, string message, int timesPerDay, int minutesBefore);

        public void DeleteNotification(Notification notification);

        public List<Notification> GetPatientNotifications(int patinetId);
    }
}
