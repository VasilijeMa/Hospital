using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public abstract class Survey
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int ServiceQuality { get; set; }
        public int SuggestToFriends { get; set; }
        public string Comment { get; set; }

        protected Survey() { }

        protected Survey(int id, string username, int serviceQuality, int suggestToFriends, string comment)
        {
            Id = id;
            Username = username;
            ServiceQuality = serviceQuality;
            SuggestToFriends = suggestToFriends;
            Comment = comment;
        }
    }
}
