using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PatientSatisfaction.Model;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IDoctorSurveyRepository
    {
        public List<DoctorSurvey> LoadAll();

        public void WriteAll();

        public void AddSurvey(string username, int doctorId, int serviceQuality, int suggestToFriends, string comment);
        
        public List<DoctorSurvey> GetByDoctorId(int id);
        public List<string> GetComments(int id);

        public List<Rating> GetRatings(int id);
    }
}
