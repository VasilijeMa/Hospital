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
            using StreamReader reader = new("./../../../../ZdravoCorp/data/doctorSurvey.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<DoctorSurvey>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(_doctorSurveys, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/doctorSurvey.json", json);
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

        public DoctorSurvey GetById(int id)
        {
            return _doctorSurveys.FirstOrDefault(doctorSurvey => doctorSurvey.Id == id);
        }

        public void AddSurvey(int appointmentId, string username, int doctorId, int serviceQuality, int suggestToFriends, string comment)
        {
            _doctorSurveys.Add(new DoctorSurvey(appointmentId, username, doctorId, serviceQuality, suggestToFriends, comment));
            WriteAll();
        }

        public void UpdateSurvey(int appointmentId, int serviceQuality, int suggestToFriends, string comment)
        {
            DoctorSurvey doctorSurveyTemp = GetById(appointmentId);
            doctorSurveyTemp.Comment = comment;
            doctorSurveyTemp.ServiceQuality = serviceQuality;
            doctorSurveyTemp.SuggestToFriends = suggestToFriends;
            WriteAll();
        }
    }
}
