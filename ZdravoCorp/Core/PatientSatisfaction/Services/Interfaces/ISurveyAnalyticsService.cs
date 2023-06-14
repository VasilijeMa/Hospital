using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientSatisfaction.Model;

namespace ZdravoCorp.Core.PatientSatisfaction.Services.Interfaces
{
    public interface ISurveyAnalyticsService
    {
        public List<string> GetComments();

        public List<Rating> GetRatings();
    }
}
