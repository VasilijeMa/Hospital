using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.PatientSatisfaction.View;
using ZdravoCorp.GUI.PatientSatisfaction.ViewModel;

namespace ZdravoCorp.Core.PatientSatisfaction.Commands
{
    public class DoctorSurveyAnalyticsCommand : BaseCommand
    {
        private DoctorRankingsViewModel _viewModel;

        public DoctorSurveyAnalyticsCommand(DoctorRankingsViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override void Execute(object? parameter)
        {
            SurveyAnalyticsView newWindow = new SurveyAnalyticsView(_viewModel.GetSelectedDoctor());
            newWindow.ShowDialog();
        }
    }
}
