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

        public List<string> GetComments()
        {
            List<string> comments = new List<string>();
            foreach (HospitalSurvey survey in _hospitalSurveys)
            {
                if (string.IsNullOrEmpty(survey.Comment))
                {
                    continue;
                }
                comments.Add("Patient " + survey.Username + " says:\n" + survey.Comment);
            }
            if (comments.Count == 0)
            {
                comments.Add("No one has yet commented on the hospital's services!");
            }
            return comments;
        }

        public List<Rating> GetRatings()
        {
            List<Rating> ratings = new List<Rating>();
            List<int> serviceRatings = new List<int> { 0, 0, 0, 0, 0 };
            List<int> recommendRatings = new List<int> { 0, 0, 0, 0, 0 };
            List<int> cleanlinessRatings = new List<int> { 0, 0, 0, 0, 0 };
            foreach (HospitalSurvey survey in _hospitalSurveys)
            {
                serviceRatings[survey.ServiceQuality - 1]++;
                recommendRatings[survey.SuggestToFriends - 1]++;
                cleanlinessRatings[survey.Cleanness - 1]++;
            }
            ratings.Add(new Rating("Service Quality", serviceRatings));
            ratings.Add(new Rating("Recommend to friends", recommendRatings));
            ratings.Add(new Rating("Cleanliness", cleanlinessRatings));
            return ratings;
        }
    }
}
