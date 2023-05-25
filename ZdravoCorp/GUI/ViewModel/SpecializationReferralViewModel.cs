using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.Core.Repositories;

namespace ZdravoCorp.GUI.ViewModel
{
    public class SpecializationReferralViewModel : INotifyPropertyChanged
    {
        private ICommand _refereCommand;
        private DoctorRepository doctorRepository = Singleton.Instance.DoctorRepository;
        private ObservableCollection<Doctor> doctors;
        private Doctor selectedDoctor = null;
        private Specialization? selectedSpecialization = null;
        private bool isSpecialization = false;
        private bool isDoctor = false;
        public Appointment Appointment { get; private set; }
        private ObservableCollection<Specialization> specializations;
        public ObservableCollection<Specialization> Specializations
        {
            get { return specializations; }
            set
            {
                specializations = value;
                OnPropertyChanged(nameof(Specializations));
            }
        }

        public ObservableCollection<Doctor> Doctors
        {
            get { return doctors; }
            set
            {
                doctors = value;
                OnPropertyChanged(nameof(Doctors));
            }
        }

        public Doctor SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsSpecialization
        {
            get { return isSpecialization; }
            set
            {
                isSpecialization = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsDoctor
        {
            get { return isDoctor; }
            set
            {
                isDoctor = value;
                OnPropertyChanged();
            }
        }
        public Specialization? SelectedSpecialization
        {
            get { return selectedSpecialization; }
            set
            {
                selectedSpecialization = value;
                OnPropertyChanged(nameof(SelectedSpecialization));
            }
        }
        public SpecializationReferralViewModel(Appointment appointment)
        {
            Appointment = appointment;
            doctors = new ObservableCollection<Doctor>(doctorRepository.Doctors);
            specializations = new ObservableCollection<Specialization>(doctorRepository.GetSpecializationOfDoctors());
        }

        public ICommand ReferCommand
        {
            get { return _refereCommand ??= new SpecializationReferralCommand(this); }
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
