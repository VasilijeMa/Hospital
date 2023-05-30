using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.View.Patient;

namespace ZdravoCorp.GUI.ViewModel
{
    public class PatientNotificationsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private NotificationService _notificationService;
        private ICommand _addCommand;
        private ICommand _updateCommand;
        private ICommand _deleteCommand;
        private bool _enable;
        private ObservableCollection<Notification> _notifications;
        private Notification _notification = null;
        public Patient Patient { get; set; }

        public ICommand AddCommand
        {
            get { return _addCommand ??= new AddNotificationCommand(this); }
        }

        public ICommand UpdateCommand
        {
            get { return _updateCommand ??= new UpdateNotificationCommand(this); }
        }

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ??= new DeleteNotificationCommand(this); }
        }

        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                OnPropertyChanged(nameof(Enable));
            }
        }

        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications; }
            set
            {
                _notifications = value;
                OnPropertyChanged(nameof(Notifications));
            }
        }

        public Notification Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                OnPropertyChanged(nameof(Notification));
                UpdateButtonEnabled();
            }
        }

        private void UpdateButtonEnabled()
        {
            Enable = Notification != null;
        }

        public PatientNotificationsViewModel(Patient patient)
        {
            _notificationService = new NotificationService(patient.Id);
            Notifications =
                new ObservableCollection<Notification>(_notificationService.GetPatientNotifications());
            Patient = patient;
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
