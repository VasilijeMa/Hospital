using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientSatisfaction.Model;

namespace ZdravoCorp.Core.PatientSatisfaction.Repositories.Interfaces
{
    public interface IHospitalSurveyRepository
    {
        public List<HospitalSurvey> LoadAll();

        public void WriteAll();

        public void AddSurvey(string username, int serviceQuality, int cleanness, int suggestToFriends, string comment);

        public List<Rating> GetRatings();

        public List<string> GetComments();
    }
}
