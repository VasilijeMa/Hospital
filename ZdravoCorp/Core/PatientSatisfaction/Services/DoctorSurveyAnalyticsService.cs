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
    public class DoctorSurveyAnalyticsService : ISurveyAnalyticsService
    {
        private int _doctorId;
        private IDoctorSurveyRepository _doctorSurveyRepository;

        public DoctorSurveyAnalyticsService(int doctorId)
        {
            _doctorId = doctorId;
            _doctorSurveyRepository = Institution.Instance.DoctorSurveyRepository;
        }
        public List<string> GetComments()
        {
            return _doctorSurveyRepository.GetComments(_doctorId);
        }

        public List<Rating> GetRatings()
        {
            return _doctorSurveyRepository.GetRatings(_doctorId);
        }
    }
}
