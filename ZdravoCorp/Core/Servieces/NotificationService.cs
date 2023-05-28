using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class NotificationService
    {
        private INotificationRepository _notificationRepository;
        private List<Notification> _notifications;
        private CancellationTokenSource _cancellationTokenSource;
        private int patientId;

        public NotificationService(int patientId)
        {
            this.patientId = patientId;
            _notificationRepository = Singleton.Instance.NotificationRepository;
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            Thread thread = new Thread(() => NotificationThread(cancellationToken));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }

        private void NotificationThread(CancellationToken cancellationToken)
        {
            while (true)
            {
                _notifications = _notificationRepository.GetPatientNotifications(patientId);
                GoThroughNotifications();
                Thread.Sleep(60000); // 1 minute interval
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private void GoThroughNotifications()
        {
            DateTime now = DateTime.Now;
            foreach (var notification in _notifications)
            {
                if (notification.Date == null) CheckMedicamentNotification(notification, now);
                else CheckPatientNotification(notification, now);
            }
        }

        private void CheckPatientNotification(Notification notification, DateTime now)
        {
            if (notification.Date.Value.Date == now.Date && notification.IsActive)
            {
                MessageBox.Show(notification.Message);
                notification.IsActive = false;
                _notificationRepository.WriteAll();
            }
        }

        private static void CheckMedicamentNotification(Notification notification, DateTime now)
        {
            int minutes = 24 * 60 / notification.TimesPerDay;
            if (((int)now.TimeOfDay.TotalMinutes - notification.MinutesBefore) % minutes == 0)
            {
                MessageBox.Show(notification.Message);
            }
        }

        public List<Notification> GetPatientNotifications()
        {
            return _notificationRepository.GetPatientNotifications(patientId);
        }

        public void CreateNotification(string message, int patientId, int timesPerDay, int minutesBefore,
            DateTime? date)
        {
            _notificationRepository.CreateNotification(message, patientId, timesPerDay, minutesBefore, date);
        }

        public void DeleteNotification(Notification notification)
        {
            _notificationRepository.DeleteNotification(notification);
        }
    }
}
