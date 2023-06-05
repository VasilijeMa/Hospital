using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
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
