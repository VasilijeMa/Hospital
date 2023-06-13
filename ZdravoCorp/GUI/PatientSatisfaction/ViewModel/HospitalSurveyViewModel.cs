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
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.GUI.View.Patient;

namespace ZdravoCorp.GUI.PatientSatisfaction.ViewModel
{
    public class HospitalSurveyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public HospitalSurveyService hospitalSurveyService;
        private ICommand _submitCommand;
        private int _serviceQuality = 1;
        private int _cleanness = 1;
        private int _suggestToFriends = 1;
        private string _comment;

        public User User { get; set; }
        public HospitalSurveyView View { get; set; }

        public ICommand SubmitCommand
        {
            get { return _submitCommand ??= new SubmitHospitalSurveyCommand(this); }
        }

        public int ServiceQuality
        {
            get { return _serviceQuality; }
            set
            {
                _serviceQuality = value;
                OnPropertyChanged(nameof(ServiceQuality));
            }
        }

        public int Cleanness
        {
            get { return _cleanness; }
            set
            {
                _cleanness = value;
                OnPropertyChanged(nameof(Cleanness));
            }
        }

        public int SuggestToFriends
        {
            get { return _suggestToFriends; }
            set
            {
                _suggestToFriends = value;
                OnPropertyChanged(nameof(SuggestToFriends));
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        public HospitalSurveyViewModel(User user, HospitalSurveyView view, HospitalSurveyService hospitalSurveyService)
        {
            this.hospitalSurveyService = hospitalSurveyService;
            User = user;
            View = view;
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
