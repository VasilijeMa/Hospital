using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class SearchDoctorCommand:BaseCommand
    {
        SearchDoctorViewModel viewModel;
        private DoctorRepository doctorRepository = new DoctorRepository();
        public SearchDoctorCommand(SearchDoctorViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public override void Execute(object? parameter)
        {
            List<Doctor> doctors = doctorRepository.SearchDoctors(viewModel.SearchBy.Trim().ToUpper());
            viewModel.FillData(doctors);
        }
    }
}
