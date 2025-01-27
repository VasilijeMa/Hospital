﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Collections.ObjectModel;
using MessageBox = System.Windows.Forms.MessageBox;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Commands
{
    public class ShowReferralsCommand : BaseCommand
    {
        private ExaminationService examinationService = new ExaminationService();
        public override bool CanExecute(object? parameter)
        {
            if (hospitalTreatmentViewModel.PatientUsername != null)
            {
                return true;
            }
            return false;
        }

        private HospitalTreatmentViewModel hospitalTreatmentViewModel;
        public override void Execute(object? parameter)
        {
            List<int> examinationsIds = examinationService.GetExaminationsIdsForHospitalizationReferral(hospitalTreatmentViewModel.PatientUsername.Username);
            if (examinationsIds.Count() != 0)
            {
                hospitalTreatmentViewModel.Examinations = new ObservableCollection<int>(examinationsIds);
            }
            else
            {
                MessageBox.Show("This patient doesn't have hospitalization referrals.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
            }
        }
        public ShowReferralsCommand(HospitalTreatmentViewModel hospitalTreatmentViewModel)
        {
            this.hospitalTreatmentViewModel = hospitalTreatmentViewModel;
        }
    }
}
