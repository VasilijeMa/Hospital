using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.VacationRequest.Model;

namespace ZdravoCorp.Core.VacationRequest.Repositories.Interfaces
{
    public interface ICancellationNotificationRepository
    {
        public List<CancellationNotification> LoadAll();
        public void SaveAll();
        public void AddAll(List<CancellationNotification> notifications);
        public List<Appointment> ExecuteNotifications(int patientId);
    }
}
