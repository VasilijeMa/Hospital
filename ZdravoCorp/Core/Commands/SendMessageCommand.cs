using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class SendMessageCommand : BaseCommand
    {
        private ChatsViewModel viewModel;
        private ChatService chatService = new ChatService();
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
            viewModel.Messages = new ObservableCollection<Message>(chatService.AddMessage(new Message(viewModel.User.Username, viewModel.Message, DateTime.Now),
                viewModel.User, viewModel.SelectedUser));
            viewModel.Message = "";
            chatService.WriteAll();
        }
    }
}
