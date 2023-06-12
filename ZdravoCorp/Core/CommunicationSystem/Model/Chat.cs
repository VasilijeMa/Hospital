using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.CommunicationSystem.Model
{
    public class Chat
    {
        public string FirstId { get; set; }
        public string SecondId { get; set; }
        public List<Message> Messages { get; set; }

        public Chat() { }

        public Chat(string firstId, string secondId, List<Message> messages)
        {
            FirstId = firstId;
            SecondId = secondId;
            Messages = messages;
        }
    }
}
