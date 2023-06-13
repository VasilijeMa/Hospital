using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.CommunicationSystem.Model;
using ZdravoCorp.Core.CommunicationSystem.Repositories.Interfaces;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.CommunicationSystem.Repositories
{
    public class ChatsRepository : IChatRepository
    {
        private List<Chat> chats;

        public List<Chat> Chats { get { return chats; } }

        public ChatsRepository()
        {
            chats = LoadAll();
        }

        public List<Chat> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/chats.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Chat>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(chats, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/chats.json", json);
        }

        public Chat GetChat(string firstId, string secondId)
        {
            return chats.FirstOrDefault(chat => chat.FirstId.Equals(firstId) && chat.SecondId.Equals(secondId) || chat.FirstId.Equals(secondId) && chat.SecondId.Equals(firstId));
        }

        public List<Message> AddMessage(Message message, User user, User selectedUser)
        {
            Chat chat = GetChat(user.Username, selectedUser.Username);
            if (chat == null)
            {
                chat = new Chat(user.Username, selectedUser.Username, new List<Message>());
                chats.Add(chat);
            }
            chat.Messages.Add(message);
            return chat.Messages;
        }
    }
}
