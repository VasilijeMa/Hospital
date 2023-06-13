using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientSatisfaction.Model;
using ZdravoCorp.Core.PatientSatisfaction.Services;

namespace ZdravoCorp.GUI.PatientSatisfaction.ViewModel
{
    public class SurveyAnalyticsViewModel
    {
        public List<Rating> Ratings { get; set; }
        public List<string> Comments { get; set; }
        public SurveyAnalyticsViewModel(string doctor)
        {
            SurveyAnalyticsService surveyAnalyticsService = new SurveyAnalyticsService();
            Ratings = surveyAnalyticsService.GetRatings(doctor);
            Comments = surveyAnalyticsService.GetComments(doctor);
        }
    }
}
