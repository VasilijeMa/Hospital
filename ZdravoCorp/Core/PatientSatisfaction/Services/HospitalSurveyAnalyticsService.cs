using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientSatisfaction.Model;
using ZdravoCorp.Core.PatientSatisfaction.Repositories.Interfaces;
using ZdravoCorp.Core.PatientSatisfaction.Services.Interfaces;

namespace ZdravoCorp.Core.PatientSatisfaction.Services
{
    public class HospitalSurveyAnalyticsService : ISurveyAnalyticsService
    {
        private IHospitalSurveyRepository _hospitalSurveyRepository;

        public HospitalSurveyAnalyticsService()
        {
            _hospitalSurveyRepository = Institution.Instance.HospitalSurveyRepository;
        }
        public List<string> GetComments()
        {
            return _hospitalSurveyRepository.GetComments();
        }

        public List<Rating> GetRatings()
        {
            return _hospitalSurveyRepository.GetRatings();
        }
    }
}
