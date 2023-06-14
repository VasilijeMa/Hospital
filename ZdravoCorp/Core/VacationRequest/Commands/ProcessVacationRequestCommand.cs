using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Services;
using ZdravoCorp.GUI.VacationRequest.ViewModel;

namespace ZdravoCorp.Core.VacationRequest.Commands
{
    public class ProcessVacationRequestCommand : BaseCommand
    {
        private VacationRequestProcessingViewModel viewModel;
        private bool approve;
        private VacationRequestProcessingService service;
        public override void Execute(object? parameter)
        {
            if (viewModel.SelectedRequest < 0) return;
            (FreeDays request, List<FreeDays> remainingRequests) = viewModel.RemoveRequest();
            service.AddProcessedRequest(request, approve);
            service.SaveRemainingRequests(remainingRequests);
            if (!approve)
            {
                return;
            }
            service.CancelAppointments(request.DoctorId, request.StartDate, request.Duration);
        }
        public ProcessVacationRequestCommand(VacationRequestProcessingViewModel viewModel, bool approve, VacationRequestProcessingService service)
        {
            this.viewModel = viewModel;
            this.approve = approve;
            this.service = service;
        }
    }
}
