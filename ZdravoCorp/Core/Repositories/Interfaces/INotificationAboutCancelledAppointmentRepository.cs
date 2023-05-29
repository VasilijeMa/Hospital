﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface INotificationAboutCancelledAppointmentRepository
    {
        public List<NotificationAboutCancelledAppointment> LoadAll();

        public void WriteAll(List<NotificationAboutCancelledAppointment> notifications);

        public void Add(NotificationAboutCancelledAppointment notificationAboutCancelledAppointment);

        public List<NotificationAboutCancelledAppointment> GetNotifications();
    }
}