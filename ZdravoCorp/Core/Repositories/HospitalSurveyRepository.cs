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
    public class HospitalSurveyRepository : IHospitalSurveyRepository
    {
        private List<HospitalSurvey> _hospitalSurveys;

        public HospitalSurveyRepository()
        {
            _hospitalSurveys = LoadAll();
        }

        public List<HospitalSurvey> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/hospitalSurvey.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<HospitalSurvey>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(_hospitalSurveys, Formatting.Indented);
            File.WriteAllText("./../../../data/hospitalSurvey.json", json);
        }

        public int getLastId()
        {
            try
            {
                return _hospitalSurveys.Max(survey => survey.Id);
            }
            catch
            {
                return 0;
            }
        }
        
        public void AddSurvey(string username, int serviceQuality, int cleanness, int suggestToFriends, string comment)
        {
            _hospitalSurveys.Add(new HospitalSurvey(getLastId() + 1, username, serviceQuality, cleanness, suggestToFriends, comment));
            WriteAll();
        }
    }
}
