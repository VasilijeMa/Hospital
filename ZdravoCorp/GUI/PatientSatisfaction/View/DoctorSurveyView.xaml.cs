﻿using System.Windows;
using ZdravoCorp.Core.PatientSatisfaction.Services;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.GUI.PatientSatisfaction.ViewModel;

namespace ZdravoCorp.GUI.View.Patient
{
    /// <summary>
    /// Interaction logic for DoctorSurveyView.xaml
    /// </summary>
    public partial class DoctorSurveyView : Window
    {
        public DoctorSurveyView(Core.UserManager.Model.Patient patient, Appointment appointment, DoctorSurveyService doctorSurveyService)
        {
            InitializeComponent();
            DataContext = new DoctorSurveyViewModel(patient, appointment, this, doctorSurveyService);
        }
    }
}
