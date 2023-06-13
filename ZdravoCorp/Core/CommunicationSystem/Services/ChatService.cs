using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.CommunicationSystem.Model;
using ZdravoCorp.Core.CommunicationSystem.Repositories.Interfaces;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.CommunicationSystem.Services
{
    public class ChatService
    {
        private IChatRepository _chatRepository;

        public ChatService()
        {
            _chatRepository = Singleton.Instance.ChatRepository;
        }

        public Chat GetChat(string firstId, string secondId)
        {
            return _chatRepository.GetChat(firstId, secondId);
        }

        public List<Message> AddMessage(Message message, User user, User selectedUser)
        {
            return _chatRepository.AddMessage(message, user, selectedUser);
        }

        public void WriteAll()
        {
            _chatRepository.WriteAll();
        }
    }
}
