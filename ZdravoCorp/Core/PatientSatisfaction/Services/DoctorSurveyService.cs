using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PatientSatisfaction.Model;
using ZdravoCorp.Core.PatientSatisfaction.Repositories.Interfaces;

namespace ZdravoCorp.Core.PatientSatisfaction.Services
{
    public class DoctorSurveyService
    {
        IDoctorSurveyRepository _doctorSurveyRepository;

        public DoctorSurveyService()
        {
            _doctorSurveyRepository = Singleton.Instance.DoctorSurveyRepository;
        }

        public void AddSurvey(int appointmentId, string username, int doctorId, int serviceQuality, int suggestToFriends, string comment)
        {
            _doctorSurveyRepository.AddSurvey(appointmentId, username, doctorId, serviceQuality, suggestToFriends, comment);
        }

        public DoctorSurvey GetById(int id)
        {
            return _doctorSurveyRepository.GetById(id);
        }

        public void UpdateSurvey(int appointmentId, int serviceQuality, int suggestToFriends, string comment)
        {
            _doctorSurveyRepository.UpdateSurvey(appointmentId, serviceQuality, suggestToFriends, comment);
        }
    }
}
