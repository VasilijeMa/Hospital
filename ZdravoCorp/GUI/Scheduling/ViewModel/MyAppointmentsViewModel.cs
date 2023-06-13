using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.PatientSatisfaction.Commands;
using ZdravoCorp.Core.PatientSatisfaction.Services;
using ZdravoCorp.Core.Scheduling.Commands;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;

namespace ZdravoCorp.GUI.Scheduling.ViewModel
{
    public class MyAppointmentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ScheduleService scheduleService = new ScheduleService();
        public DoctorSurveyService doctorSurveyService;

        private ICommand _updateCommand;
        private ICommand _cancelCommand;
        private ICommand _surveyCommand;
        private List<Appointment> _appointments;
        private Appointment _selectedAppointment;

        public Patient Patient { get; set; }
        public MyAppointmentsWindow View { get; set; }

        public ICommand UpdateCommand
        {
            get { return _updateCommand ??= new UpdateAppointmentCommand(this); }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand ??= new CancelAppointmentCommand(this); }
        }

        public ICommand SurveyCommand
        {
            get { return _surveyCommand ??= new DoctorSurveyCommand(this); }
        }

        public List<Appointment> Appointments
        {
            get { return _appointments; }
            set
            {
                _appointments = value;
                OnPropertyChanged(nameof(Appointments));
            }
        }

        public Appointment SelectedAppointment
        {
            get { return _selectedAppointment; }
            set
            {
                _selectedAppointment = value;
                OnPropertyChanged(nameof(SelectedAppointment));
                ((BaseCommand)UpdateCommand).RaiseCanExecuteChanged();
                ((BaseCommand)CancelCommand).RaiseCanExecuteChanged();
                ((BaseCommand)SurveyCommand).RaiseCanExecuteChanged();
            }
        }

        public MyAppointmentsViewModel(Patient patient, MyAppointmentsWindow view, DoctorSurveyService doctorSurveyService)
        {
            this.doctorSurveyService = doctorSurveyService;
            Patient = patient;
            View = view;
            Appointments = scheduleService.GetAppointmentsForPatient(patient.Id);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
