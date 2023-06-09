using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class HospitalSurvey
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int ServiceQuality { get; set; }
        public int Cleanness { get; set; }
        public int SuggestToFriends { get; set; }
        public string Comment { get; set; }

        public HospitalSurvey() { }

        public HospitalSurvey(int id, string username, int serviceQuality, int cleanness, int suggestToFriends, string comment)
        {
            Id = id;
            Username = username;
            ServiceQuality = serviceQuality;
            Cleanness = cleanness;
            SuggestToFriends = suggestToFriends;
            Comment = comment;
        }
    }
}
