using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;
using ZdravoCorp.GUI.DoctorPatientSearch.ViewModel;

namespace ZdravoCorp.Core.DoctorPatientSearch.Commands
{
    public class SearchDoctorCommand : BaseCommand
    {
        SearchDoctorViewModel viewModel;
        private DoctorService doctorService = new DoctorService();

        public SearchDoctorCommand(SearchDoctorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            List<Doctor> doctors = doctorService.SearchDoctors(viewModel.SearchBy.Trim().ToUpper());
            viewModel.FillData(doctors);
        }
    }
}
