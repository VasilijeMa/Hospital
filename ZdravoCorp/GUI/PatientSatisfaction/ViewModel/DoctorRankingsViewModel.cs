using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
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
        public DoctorRankingsViewModel()
        {
            ShowSurveys = new DoctorSurveyAnalyticsCommand(this);
            SurveyAnalyticsService surveyAnalyticsService = new SurveyAnalyticsService();
            DoctorNames = surveyAnalyticsService.GetDoctorNames();
            LowestRatedDoctors = surveyAnalyticsService.GetRankedDoctors(DoctorNames, false);
            HighestRatedDoctors = surveyAnalyticsService.GetRankedDoctors(DoctorNames, true);
        }

        public string GetSelectedDoctor()
        {
            return DoctorNames[SelectedDoctor];
        }
    }
}
