using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.UserManager.Services;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Repositories.Interfaces;
using ZdravoCorp.GUI.VacationRequest.ViewModel;

namespace ZdravoCorp.Core.VacationRequest.Commands
{
    internal class FreeDaysCommand : BaseCommand
    {
        private FreeDaysViewModel viewModel;
        private DoctorService DoctorService = new DoctorService();
        //private IFreeDaysRepository freeDaysRepository = Institution.Instance.FreeDaysRepository;
        public FreeDaysCommand(FreeDaysViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (!IsDoctorFree())
            {
                return;
            }
            MessageBox.Show("Successfully get free days");
            FreeDays freeDays = new FreeDays(viewModel.Doctor.Id, viewModel.StartDate, viewModel.Duration, viewModel.Reason);
            viewModel.freeDaysService.AddFreeDays(freeDays);
        }

        private bool IsDoctorFree()
        {
            TimeSlot timeSlot = new TimeSlot(viewModel.StartDate, viewModel.Duration * 1440);
            DateTime days = viewModel.StartDate.AddDays(2);
            if (!DoctorService.IsAvailable(timeSlot, viewModel.Doctor.Id))
            {
                MessageBox.Show("You have appointments scheduled during that period.");
                return false;
            }
            return true;
        }
    }
}
