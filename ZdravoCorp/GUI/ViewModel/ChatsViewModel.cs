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

namespace ZdravoCorp.GUI.ViewModel
{
    public class ChatsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ChatService _chatService;
        private UserService _userService;
        private ICommand _sendCommand;
        private string _message = "";
        private ObservableCollection<User> _users;
        private ObservableCollection<Message> _messages;
        private User _selectedUser;
        public User User { get; set; }

        public ICommand SendCommand
        {
            get { return _sendCommand ??= new SendMessageCommand(this); }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
                ((BaseCommand)SendCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                LoadChat();
                ((BaseCommand)SendCommand).RaiseCanExecuteChanged();
            }
        }

        public ChatsViewModel(User user)
        {
            User = user;
            _userService = new UserService();
            _chatService = new ChatService();
            Users = new ObservableCollection<User>(_userService.GetNursesAndDoctors(user.Username));
            Messages = new ObservableCollection<Message>();
            //Messages = new ObservableCollection<Message> {
            //    new Message{Username = "vanja",Content = "JAOOO", Created = new DateTime(2023, 6, 1)},
            //    new Message{Username = "vanja", Content = "aaaa", Created = new DateTime(2023, 6, 2)},
            //    new Message{Username = "vanja", Content = "uzas", Created = new DateTime(2023, 6, 3)},
            //};
        }

        private void LoadChat()
        {
            Chat chat = _chatService.GetChat(User.Username, SelectedUser.Username);
            Messages.Clear();
            if (chat != null)
            {
                Messages = new ObservableCollection<Message>(chat.Messages);
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
