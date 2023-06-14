using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.PatientNotification.Commands;
using ZdravoCorp.Core.PatientNotification.Model;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.GUI.View.Patient;

namespace ZdravoCorp.GUI.PatientNotification.ViewModel
{
    public class NotificationFormViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ICommand _submitCommand;
        private string _message;
        private int _timesPerDay;
        private int _minutesBefore;
        private DateTime _date;

        public Patient Patient { get; set; }
        public Notification Notification { get; set; }
        public NotificationFormView View { get; set; }

        public ICommand SubmitCommand
        {
            get { return _submitCommand ??= new SubmitNotificationCommand(this); }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public int TimesPerDay
        {
            get { return _timesPerDay; }
            set
            {
                _timesPerDay = value;
                OnPropertyChanged(nameof(TimesPerDay));
            }
        }

        public int MinutesBefore
        {
            get { return _minutesBefore; }
            set
            {
                _minutesBefore = value;
                OnPropertyChanged(nameof(MinutesBefore));
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public NotificationFormViewModel(Patient patient, NotificationFormView view, Notification notification = null)
        {
            Patient = patient;
            Notification = notification;
            View = view;
            Date = DateTime.Now;
            if (notification != null)
            {
                Message = notification.Message;
                TimesPerDay = notification.TimesPerDay;
                MinutesBefore = notification.MinutesBefore;
            }
        }

        public bool IsValid()
        {
            return Date > DateTime.Now && Message != "" && Message != null && MinutesBefore >= 0;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
