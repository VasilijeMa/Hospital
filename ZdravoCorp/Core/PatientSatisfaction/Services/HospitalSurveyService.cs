using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PatientSatisfaction.Repositories.Interfaces;
using ZdravoCorp.Core.Repositories;

namespace ZdravoCorp.Core.PatientSatisfaction.Services
{
    public class HospitalSurveyService
    {
        IHospitalSurveyRepository _hospitalSurveyRepository;

        public HospitalSurveyService(IHospitalSurveyRepository hospitalSurveyRepository)
        {
            _hospitalSurveyRepository = hospitalSurveyRepository;
        }

        public void AddSurvey(string username, int serviceQuality, int cleanness, int suggestToFriends, string comment)
        {
            _hospitalSurveyRepository.AddSurvey(username, serviceQuality, cleanness, suggestToFriends, comment);
        }
    }
}
