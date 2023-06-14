using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.PatientSatisfaction.Commands;
using ZdravoCorp.Core.PatientSatisfaction.Services;

namespace ZdravoCorp.GUI.PatientSatisfaction.ViewModel
{
    public class DoctorRankingsViewModel
    {
        public List<string> DoctorNames { get; set; }
        public int SelectedDoctor { get; set; } = 0;
        public ICommand ShowSurveys { get; }
        public List<string> LowestRatedDoctors { get; set; }

        public List<string> HighestRatedDoctors { get; set; }
        private int ExtractID(string doctor)
        {
            return int.Parse(doctor.Split(':')[1]);
        }
        public DoctorRankingsViewModel()
        {
            ShowSurveys = new DoctorSurveyAnalyticsCommand(this);
            DoctorRankingsService surveyAnalyticsService = new DoctorRankingsService();
            DoctorNames = surveyAnalyticsService.GetDoctorNames();
            LowestRatedDoctors = surveyAnalyticsService.GetRankedDoctors(false);
            HighestRatedDoctors = surveyAnalyticsService.GetRankedDoctors(true);
        }

        public int GetSelectedDoctor()
        {
            return ExtractID(DoctorNames[SelectedDoctor]);
        }
    }
}
