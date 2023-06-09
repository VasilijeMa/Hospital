using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class HospitalSurveyService
    {
        IHospitalSurveyRepository _hospitalSurveyRepository;

        public HospitalSurveyService()
        {
            _hospitalSurveyRepository = Singleton.Instance.HospitalSurveyRepository;
        }

        public void AddSurvey(string username, int serviceQuality, int cleanness, int suggestToFriends, string comment)
        {
            _hospitalSurveyRepository.AddSurvey(username, serviceQuality, cleanness, suggestToFriends, comment);
        }
    }
}
