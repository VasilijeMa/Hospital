using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientSatisfaction.Model;
using ZdravoCorp.Core.PatientSatisfaction.Repositories.Interfaces;

namespace ZdravoCorp.Core.PatientSatisfaction.Repositories
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
        public List<DoctorSurvey> GetByDoctorId(int id)
        {
            List<DoctorSurvey> surveys = new List<DoctorSurvey>();
            foreach (DoctorSurvey survey in _doctorSurveys)
            {
                if (survey.DoctorId != id)
                {
                    continue;
                }
                surveys.Add(survey);
            }
            return surveys;
        }

        public List<string> GetComments(int id)
        {
            List<DoctorSurvey> surveysWithID = GetByDoctorId(id);
            List<string> comments = new List<string>();
            foreach (DoctorSurvey survey in surveysWithID)
            {
                if (string.IsNullOrEmpty(survey.Comment))
                {
                    continue;
                }
                comments.Add("Patient " + survey.Username + " says:\n" + survey.Comment);
            }
            if (comments.Count == 0)
            {
                comments.Add("No one has yet commented on the doctor's services!");
            }
            return comments;
        }
        public List<Rating> GetRatings(int id)
        {
            List<DoctorSurvey> surveysWithID = GetByDoctorId(id);
            List<Rating> ratings = new List<Rating>();
            List<int> serviceRatings = new List<int> { 0, 0, 0, 0, 0 };
            List<int> recommendRatings = new List<int> { 0, 0, 0, 0, 0 };
            foreach (DoctorSurvey survey in surveysWithID)
            {
                serviceRatings[survey.ServiceQuality - 1]++;
                recommendRatings[survey.SuggestToFriends - 1]++;
            }
            ratings.Add(new Rating("Service Quality", serviceRatings));
            ratings.Add(new Rating("Recommend to friends", recommendRatings));
            return ratings;
        }
    }
}
