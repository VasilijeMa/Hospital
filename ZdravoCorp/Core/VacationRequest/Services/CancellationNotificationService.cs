using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Repositories;
using ZdravoCorp.Core.VacationRequest.Repositories.Interfaces;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZdravoCorp.Core.VacationRequest.Services
{
    public class CancellationNotificationService
    {
        private ICancellationNotificationRepository _cancellationNotificationRepository;

        public CancellationNotificationService(ICancellationNotificationRepository cancellationNotificationRepository)
        {
            _cancellationNotificationRepository = cancellationNotificationRepository;
        }

        
        public List<Appointment> ExecuteNotifications(int patientId)
        {
            return _cancellationNotificationRepository.ExecuteNotifications(patientId);
        }
    }
}
