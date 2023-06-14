using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Repositories;
using ZdravoCorp.Core.VacationRequest.Repositories.Interfaces;

namespace ZdravoCorp.Core.VacationRequest.Services
{
    public class VacationRequestProcessingService
    {
        private IFreeDaysRepository _freeDaysRepository;
        private IProcessedVacationRequestRepository _processedVacationRequestRepository;
        private IScheduleRepository _scheduleRepository;
        private ICancellationNotificationRepository _cancellationNotificationRepository;
        public VacationRequestProcessingService()
        {
            _freeDaysRepository = Institution.Instance.FreeDaysRepository;
            _scheduleRepository = Institution.Instance.ScheduleRepository;
            _processedVacationRequestRepository = new ProcessedVacationRequestRepository();
            _cancellationNotificationRepository = new CancellationNotificationRepository();
        }

        public List<FreeDays> GetRequests()
        {
            return _freeDaysRepository.GetAll();
        }

        public void AddProcessedRequest(FreeDays request, bool isApproved)
        {
            ProcessedVacationRequest processedRequest = new ProcessedVacationRequest(request.DoctorId,
                request.StartDate, request.Duration, request.Reason, isApproved);
            _processedVacationRequestRepository.Add(processedRequest);
        }

        public void SaveRemainingRequests(List<FreeDays> remainingRequests)
        {
            _freeDaysRepository.SaveAll(remainingRequests);
        }

        public void CancelAppointments(int doctorId, DateTime startDate, int duration)
        {
            TimeSlot timeSlot = new TimeSlot(startDate, duration * 1440);
            List<CancellationNotification> cancellationNotifications = _scheduleRepository.CancelAppointments(doctorId, timeSlot);
            _cancellationNotificationRepository.AddAll(cancellationNotifications);
        }
    }
}
