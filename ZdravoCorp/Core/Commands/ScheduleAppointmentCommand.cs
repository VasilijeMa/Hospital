using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    public class ScheduleAppointmentCommand:BaseCommand
    {
        SearchDoctorViewModel viewModel;
        Patient patient;

        public ScheduleAppointmentCommand(SearchDoctorViewModel viewModel, Patient patient)
        {
            this.viewModel = viewModel;
            this.patient = patient;
        }

        public override void Execute(object? parameter)
        {
            MakeAppointmentWindow makeAppointmentWindow = new MakeAppointmentWindow(patient, viewModel.SelectedDoctor);
            makeAppointmentWindow.ShowDialog();
        }
    }
}
