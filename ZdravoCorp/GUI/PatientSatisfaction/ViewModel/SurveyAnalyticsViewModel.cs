using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientSatisfaction.Model;
using ZdravoCorp.Core.PatientSatisfaction.Services;
using ZdravoCorp.Core.PatientSatisfaction.Services.Interfaces;

namespace ZdravoCorp.GUI.PatientSatisfaction.ViewModel
{
    public class SurveyAnalyticsViewModel
    {
        public List<Rating> Ratings { get; set; }
        public List<string> Comments { get; set; }
        private ISurveyAnalyticsService _service;

        public SurveyAnalyticsViewModel(int doctorId)
        {
            if (doctorId <= 0)
            {
                _service = new HospitalSurveyAnalyticsService();
            }
            else
            {
                _service = new DoctorSurveyAnalyticsService(doctorId);
            }
            Ratings = _service.GetRatings();
            Comments = _service.GetComments();
        }
    }
}
