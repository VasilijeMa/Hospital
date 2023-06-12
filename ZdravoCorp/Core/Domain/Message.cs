using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class Message
    {
        public string Username { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }

        public Message() { }

        public Message(string username, string content, DateTime created)
        {
            Username = username;
            Content = content;
            Created = created;
        }

        public override string ToString()
        {
            return Username + ": " + Content;
        }
    }
}
