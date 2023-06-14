using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientSatisfaction.Model;
using ZdravoCorp.Core.UserManager.Repositories.Interfaces;

namespace ZdravoCorp.Core.PatientSatisfaction.Services
{
    public class DoctorRankingsService
    {
        private IDoctorRepository _doctorRepository;
        private int ExtractID(string doctor)
        {
            return int.Parse(doctor.Split(':')[1]);
        }
        public DoctorRankingsService()
        {
            _doctorRepository = Institution.Instance.DoctorRepository;
        }
        public List<string> GetDoctorNames()
        {
            return _doctorRepository.GetFullNames();
        }

        private double CalculateAverage(List<Rating> ratings)
        {
            double sum = 0.0;
            foreach (Rating rating in ratings)
            {
                sum += rating.AverageScore;
            }
            return Math.Round(sum / ratings.Count(), 2);
        }
        private List<string> FormTotalScores(List<KeyValuePair<string, double>> rankedDoctors)
        {
            List<string> worstRatedDoctors = new List<string>();
            foreach (KeyValuePair<string, double> doctorScore in rankedDoctors)
            {
                worstRatedDoctors.Add(doctorScore.Key + ", rating: " + doctorScore.Value.ToString());
            }
            return worstRatedDoctors;
        }
        private Dictionary<string, double> CalculateRatingScores()
        {
            Dictionary<string, double> scores = new Dictionary<string, double>();
            foreach (string doctor in _doctorRepository.GetFullNames())
            {
                DoctorSurveyAnalyticsService service = new DoctorSurveyAnalyticsService(ExtractID(doctor));
                scores[doctor] = CalculateAverage(service.GetRatings());
            }
            return scores;
        }
        public List<string> GetRankedDoctors(bool best)
        {
            Dictionary<string, double> scores = CalculateRatingScores();
            List<KeyValuePair<string, double>> topThree;
            if (best)
            {
                topThree = scores.OrderByDescending(pair => pair.Value).Take(3).ToList();
                return FormTotalScores(topThree);
            }
            topThree = scores.OrderBy(pair => pair.Value).Take(3).ToList();
            return FormTotalScores(topThree);
        }
    }
}
