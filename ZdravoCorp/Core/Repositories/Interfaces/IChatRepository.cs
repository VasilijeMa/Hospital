using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public List<Chat> LoadAll();

        public void WriteAll();

        public Chat GetChat(string firstId, string secondId);

        public List<Message> AddMessage(Message message, User user, User selectedUser);
    }
}
