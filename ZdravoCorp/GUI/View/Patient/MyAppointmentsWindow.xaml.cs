using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MyAppointmentsWindow.xaml
    /// </summary>
    public partial class MyAppointmentsWindow : Window
    {
        private ScheduleService scheduleService = new ScheduleService();
        public MyAppointmentsWindow(Patient patient, DoctorSurveyService doctorSurveyService)
        {
            InitializeComponent();
            DataContext = new MyAppointmentsViewModel(patient, this, doctorSurveyService);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            scheduleService.WriteAllAppointmens();
        }

    }
}
