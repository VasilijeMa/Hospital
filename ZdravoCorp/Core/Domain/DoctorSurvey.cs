using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class DoctorSurvey : Survey
    {
        public int DoctorId { get; set; }

        public DoctorSurvey() { }

        public DoctorSurvey(int id, string username, int doctorId, int serviceQuality, int suggestToFriends, string comment) : base(id, username, serviceQuality, suggestToFriends, comment)
        {
            DoctorId = doctorId;
        }
    }
}
