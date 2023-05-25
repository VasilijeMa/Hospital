using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.GUI.ViewModel
{
    public class PatientNotificationsViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ICommand _addCommand;
        private ICommand _updateCommand;
        private ICommand _deleteCommand;
        private List<Notification> _notifications;
        private Notification _notification;

        public ICommand AddCommand
        {
            get { return _addCommand ??= new AddNotificationCommand(); }
        }

        public ICommand UpdateCommand
        {
            get { return _updateCommand ??= new UpdateNotificationCommand(); }
        }

        public ICommand DeleteCommand
        {
            get { return _deleteCommand ??= new DeleteNotificationCommand(); }
        }

        public List<Notification> Notifications
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
            }
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
