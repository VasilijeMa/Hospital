using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class SubmitHospitalSurveyCommand : BaseCommand
    {
        private HospitalSurveyService hospitalSurveyService = new HospitalSurveyService();
        private HospitalSurveyViewModel viewModel;

        public SubmitHospitalSurveyCommand(HospitalSurveyViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            hospitalSurveyService.AddSurvey(viewModel.User.Username, viewModel.ServiceQuality, viewModel.Cleanness, viewModel.SuggestToFriends, viewModel.Comment);
            viewModel.View.Close();
        }
    }
}
