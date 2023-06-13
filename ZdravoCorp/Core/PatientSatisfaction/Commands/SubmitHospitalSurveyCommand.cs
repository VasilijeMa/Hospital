using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.PatientSatisfaction.Services;
using ZdravoCorp.GUI.PatientSatisfaction.ViewModel;

namespace ZdravoCorp.Core.PatientSatisfaction.Commands
{
    public class SubmitHospitalSurveyCommand : BaseCommand
    {
        private HospitalSurveyViewModel viewModel;

        public SubmitHospitalSurveyCommand(HospitalSurveyViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            viewModel.hospitalSurveyService.AddSurvey(viewModel.User.Username, viewModel.ServiceQuality, viewModel.Cleanness, viewModel.SuggestToFriends, viewModel.Comment);
            viewModel.View.Close();
        }
    }
}
