using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IHospitalSurveyRepository
    {
        public List<HospitalSurvey> LoadAll();

        public void WriteAll();

        public void AddSurvey(string username, int serviceQuality, int purity, int suggestToFriends, string comment);
    }
}
