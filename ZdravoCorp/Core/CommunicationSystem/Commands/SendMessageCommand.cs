using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.CommunicationSystem.Model;
using ZdravoCorp.Core.CommunicationSystem.Services;
using ZdravoCorp.GUI.CommunicationSystem.ViewModel;

namespace ZdravoCorp.Core.CommunicationSystem.Commands
{
    public class SendMessageCommand : BaseCommand
    {
        private ChatsViewModel viewModel;
        public SendMessageCommand(ChatsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            if (viewModel.SelectedUser == null || viewModel.Message == "") return false;
            return true;
        }

        public override void Execute(object? parameter)
        {
            viewModel.Messages = new ObservableCollection<Message>(viewModel.chatService.AddMessage(new Message(viewModel.User.Username, viewModel.Message, DateTime.Now),
                viewModel.User, viewModel.SelectedUser));
            viewModel.Message = "";
            viewModel.chatService.WriteAll();
        }
    }
}
