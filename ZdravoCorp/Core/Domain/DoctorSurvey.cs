using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class DoctorSurvey
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int DoctorId { get; set; }
        public int ServiceQuality { get; set; }
        public int SuggestToFriends { get; set; }
        public string Comment { get; set; }

        public DoctorSurvey() { }

        public DoctorSurvey(int id, string username, int doctorId, int serviceQuality, int suggestToFriends, string comment)
        {
            Id = id;
            Username = username;
            DoctorId = doctorId;
            ServiceQuality = serviceQuality;
            SuggestToFriends = suggestToFriends;
            Comment = comment;
        }
    }
}
