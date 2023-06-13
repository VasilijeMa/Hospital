using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.PatientSatisfaction.Model;
using ZdravoCorp.Core.PatientSatisfaction.Repositories;
using ZdravoCorp.Core.PatientSatisfaction.Repositories.Interfaces;
using ZdravoCorp.Core.UserManager.Repositories;
using ZdravoCorp.Core.UserManager.Repositories.Interfaces;

namespace ZdravoCorp.Core.PatientSatisfaction.Services
{
    public class SurveyAnalyticsService
    {
        private IHospitalSurveyRepository _hospitalSurveyRepository;
        private IDoctorSurveyRepository _doctorSurveyRepository;
        private IDoctorRepository _doctorRepository;
        public SurveyAnalyticsService()
        {
            _hospitalSurveyRepository = new HospitalSurveyRepository();
            _doctorSurveyRepository = new DoctorSurveyRepository();
            _doctorRepository = new DoctorRepository();
        }
        private int ExtractID(string doctor)
        {
            return int.Parse(doctor.Split(':')[1]);
        }
        public List<string> GetComments(string doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor))
            {
                return _hospitalSurveyRepository.GetComments();
            }

            int id = ExtractID(doctor);
            return _doctorSurveyRepository.GetComments(id);
        }

        public List<Rating> GetRatings(string doctor)
        {
            if (string.IsNullOrWhiteSpace(doctor))
            {
                return _hospitalSurveyRepository.GetRatings();
            }
            int id = ExtractID(doctor);
            return _doctorSurveyRepository.GetRatings(id);
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
        private Dictionary<string, double> CalculateRatingScores(List<string> doctors)
        {
            Dictionary<string, double> scores = new Dictionary<string, double>();
            foreach (string doctor in doctors)
            {
                scores[doctor] = CalculateAverage(GetRatings(doctor));
            }
            return scores;
        }
        public List<string> GetRankedDoctors(List<string> doctors, bool best)
        {
            Dictionary<string, double> scores = CalculateRatingScores(doctors);
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
