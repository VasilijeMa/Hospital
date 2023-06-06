using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class SubmitDoctorSurveyCommand : BaseCommand
    {
        private DoctorSurveyService doctorSurveyService = new DoctorSurveyService();
        private DoctorSurveyViewModel viewModel;
        public SubmitDoctorSurveyCommand(DoctorSurveyViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public override void Execute(object? parameter)
        {
            doctorSurveyService.AddSurvey(viewModel.User.Username, viewModel.DoctorId, viewModel.ServiceQuality, viewModel.SuggestToFriends, viewModel.Comment);
            viewModel.View.Close();
        }
    }
}
