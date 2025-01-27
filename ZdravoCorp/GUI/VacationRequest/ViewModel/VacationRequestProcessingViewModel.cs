﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.VacationRequest.Commands;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Services;

namespace ZdravoCorp.GUI.VacationRequest.ViewModel
{
    public class VacationRequestProcessingViewModel
    {
        public ObservableCollection<FreeDays> VacationRequests { get; set; }
        public int SelectedRequest { get; set; } = -1;
        public ICommand ApproveCommand { get; set; }
        public ICommand DenyCommand { get; set;}

        public VacationRequestProcessingViewModel()
        {
            VacationRequests = new ObservableCollection<FreeDays>();
            VacationRequestProcessingService service = new VacationRequestProcessingService();
            List<FreeDays> requests = service.GetRequests();
            foreach (FreeDays request in requests)
            {
                VacationRequests.Add(request);
            }
            ApproveCommand = new ProcessVacationRequestCommand(this, true);
            DenyCommand = new ProcessVacationRequestCommand(this, false);
        }

        public (FreeDays, List<FreeDays>) RemoveRequest()
        {
            FreeDays request = VacationRequests[SelectedRequest];
            VacationRequests.RemoveAt(SelectedRequest);
            List<FreeDays> requests = new List<FreeDays>();
            foreach(FreeDays freeDays in VacationRequests)
            {
                requests.Add(freeDays);
            }
            SelectedRequest = -1;
            return (request, requests);
        }
    }
}
