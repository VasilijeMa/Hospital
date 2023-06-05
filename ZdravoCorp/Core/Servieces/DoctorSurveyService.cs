using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class DoctorSurveyService
    {
        IDoctorSurveyRepository _doctorSurveyRepository;

        public DoctorSurveyService()
        {
            _doctorSurveyRepository = Singleton.Instance.DoctorSurveyRepository;
        }

        public void AddSurvey(string username, int doctorId, int serviceQuality, int suggestToFriends, string comment)
        {
            _doctorSurveyRepository.AddSurvey(username, doctorId, serviceQuality, suggestToFriends, comment);
        }
    }
}
