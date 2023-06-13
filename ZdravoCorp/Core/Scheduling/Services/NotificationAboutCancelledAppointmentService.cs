using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;

namespace ZdravoCorp.Core.Scheduling.Services
{
    public class NotificationAboutCancelledAppointmentService
    {
        private INotificationAboutCancelledAppointmentRepository _repository;

        public NotificationAboutCancelledAppointmentService()
        {
            _repository = Institution.Instance.NotificationAboutCancelledAppointmentRepository;
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
