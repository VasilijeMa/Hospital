using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Commands
{
    public class ShowRoomsCommand : BaseCommand
    {

        private HospitalTreatmentViewModel hospitalTreatmentViewModel;
        private ExaminationService examinationService = new ExaminationService();
        public override void Execute(object? parameter)
        {
            Examination examination = examinationService.GetExaminationById(hospitalTreatmentViewModel.ExaminationId);
            DateOnly startDate = examination.HospitalizationRefferal.StartDate;
            hospitalTreatmentViewModel.Examination = examination;
            hospitalTreatmentViewModel.StartDate = startDate;
            int duration = examination.HospitalizationRefferal.Duration;
            DateOnly endDate = startDate.AddDays(duration);
            hospitalTreatmentViewModel.EndDate = endDate;
            hospitalTreatmentViewModel.Rooms = new ObservableCollection<string>(hospitalTreatmentViewModel.hospitalStayService.FindFreeRooms(startDate, endDate));
        }
        public override bool CanExecute(object? parameter)
        {
            if (hospitalTreatmentViewModel.ExaminationId != 0)
            {
                return true;
            }
            return false;
        }


        public ShowRoomsCommand(HospitalTreatmentViewModel hospitalTreatmentViewModel)
        {
            this.hospitalTreatmentViewModel = hospitalTreatmentViewModel;
        }
    }
}
