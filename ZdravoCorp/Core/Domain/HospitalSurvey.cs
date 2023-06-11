using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class HospitalSurvey : Survey
    {
        public int Cleanness { get; set; }

        public HospitalSurvey() { }

        public HospitalSurvey(int id, string username, int serviceQuality, int cleanness, int suggestToFriends, string comment) : base(id, username, serviceQuality, suggestToFriends, comment)
        {
            Cleanness = cleanness;
        }
    }
}
