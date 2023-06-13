using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp.Core.Commands
{
    public class ConfirmCommand : BaseCommand
    {
        private HospitalTreatmentViewModel hospitalTreatmentViewModel;
        private HospitalStayService hospitalStayService = new HospitalStayService();
        private ExaminationService examinationService = new ExaminationService();
        public override void Execute(object? parameter)

        {
            HospitalStay hospitalStay = new HospitalStay(hospitalTreatmentViewModel.ExaminationId, hospitalTreatmentViewModel.StartDate, hospitalTreatmentViewModel.EndDate, hospitalTreatmentViewModel.RoomId);
            hospitalStayService.Add(hospitalStay);
            hospitalTreatmentViewModel.Examination.HospitalizationRefferal.RoomId = hospitalTreatmentViewModel.RoomId;
            examinationService.WriteAll();
            MessageBox.Show("You have successfully allocated a room for hospital treatment.", "Information", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
        }

        public override bool CanExecute(object? parameter)
        {
            if (hospitalTreatmentViewModel.RoomId != null)
            {
                return true;
            }
            return false;
        }


        public ConfirmCommand(HospitalTreatmentViewModel hospitalTreatmentViewModel)
        {
            this.hospitalTreatmentViewModel = hospitalTreatmentViewModel;
        }
    }
}
