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

namespace ZdravoCorp.Core.VacationRequest.Services
{
    public class CancellationNotificationService
    {
        private ICancellationNotificationRepository _cancellationNotificationRepository;

        public CancellationNotificationService()
        {
            _cancellationNotificationRepository = new CancellationNotificationRepository();
        }

        
        public void CheckWindows()
        {
            IEnumerable<PatientWindow> patientWindows = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                patientWindows = Application.Current.Windows.OfType<PatientWindow>();
            });

            if (patientWindows == null || !patientWindows.Any()) return;
            foreach (PatientWindow window in patientWindows)
            {
                int patientId = 0;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    patientId = window.GetPatientId();
                    List<Appointment> cancelledAppointments = _cancellationNotificationRepository.ExecuteNotifications(patientId);
                    window.ShowNotifications(cancelledAppointments);
                });
            }
        }
    }
}
