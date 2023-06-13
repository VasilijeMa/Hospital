using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;
using ZdravoCorp.Core.VacationRequest.Commands;
using ZdravoCorp.Core.VacationRequest.Repositories.Interfaces;
using ZdravoCorp.Core.VacationRequest.Services;

namespace ZdravoCorp.GUI.VacationRequest.ViewModel
{
    public class FreeDaysViewModel : INotifyPropertyChanged
    {
        private ICommand _submitCommand;
        private int duration;
        private string reason = "";
        private DateTime startDate;
        public Doctor Doctor { get; set; }
        public DoctorService doctorService = new DoctorService();
        public FreeDaysService freeDaysService;


        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public string Reason
        {
            get { return reason; }
            set
            {
                reason = value;
                OnPropertyChanged(nameof(Reason));
            }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public FreeDaysViewModel(Doctor doctor, FreeDaysService freeDaysService)
        {
            this.freeDaysService = freeDaysService;
            startDate = DateTime.Now;
            Doctor = doctor;
        }

        public bool isValid()
        {
            if (Reason == "" || StartDate == null)
            {
                return false;
            }
            return true;
        }

        public ICommand SubmitCommand
        {
            get { return _submitCommand ??= new FreeDaysCommand(this); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
