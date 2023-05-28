using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class NotificationAboutCancelledAppointmentService
    {
        private INotificationAboutCancelledAppointmentRepository _repository;

        public NotificationAboutCancelledAppointmentService()
        {
            _repository = Singleton.Instance.NotificationAboutCancelledAppointmentRepository;
        }

        public List<NotificationAboutCancelledAppointment> GetNotifications()
        {
            return _repository.GetNotifications();
        }

        public void WriteAll(List<NotificationAboutCancelledAppointment> notifications)
        {
            _repository.WriteAll(notifications);
        }

        public void AddNotification(NotificationAboutCancelledAppointment notification)
        {
            _repository.Add(notification);
        }
    }
}
