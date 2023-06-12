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
            if (viewModel.DoctorSurvey != null)
            {
                doctorSurveyService.UpdateSurvey(viewModel.AppointmentId, viewModel.ServiceQuality, viewModel.SuggestToFriends, viewModel.Comment);
            }
            else
            {
                doctorSurveyService.AddSurvey(viewModel.AppointmentId, viewModel.User.Username, viewModel.DoctorId, viewModel.ServiceQuality, viewModel.SuggestToFriends, viewModel.Comment);
            }
            viewModel.View.Close();
        }
    }
}
