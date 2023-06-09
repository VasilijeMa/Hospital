using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Repositories
{
    public class DoctorSurveyRepository : IDoctorSurveyRepository
    {
        private List<DoctorSurvey> _doctorSurveys;

        public DoctorSurveyRepository()
        {
            _doctorSurveys = LoadAll();
        }

        public List<DoctorSurvey> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/doctorSurvey.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<DoctorSurvey>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(_doctorSurveys, Formatting.Indented);
            File.WriteAllText("./../../../data/doctorSurvey.json", json);
        }

        public int getLastId()
        {
            try
            {
                return _doctorSurveys.Max(survey => survey.Id);
            }
            catch
            {
                return 0;
            }
        }

        public void AddSurvey(string username, int doctorId, int serviceQuality, int suggestToFriends, string comment)
        {
            _doctorSurveys.Add(new DoctorSurvey(getLastId() + 1, username, doctorId, serviceQuality, suggestToFriends, comment));
            WriteAll();
        }
    }
}
