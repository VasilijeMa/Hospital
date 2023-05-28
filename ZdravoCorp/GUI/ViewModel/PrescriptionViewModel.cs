using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ZdravoCorp.Core.Commands;
using System.Windows.Input;
using ZdravoCorp.Core.Domain;
using System.Collections.ObjectModel;
using ZdravoCorp.Core.Domain.Enums;

namespace ZdravoCorp.GUI.ViewModel
{
    internal class PrescriptionViewModel : INotifyPropertyChanged
    {
        private ICommand _prescriptionCommand;
        public ObservableCollection<Medicament> medicaments;
        public ObservableCollection<TimeForMedicament> timeForMedicaments;
        public Medicament selectedMedicament;
        public TimeForMedicament? selectedTime = null;
        public Appointment Appointment { get; set; }
        private int perDay;

        public ObservableCollection<Medicament> Medicaments
        {
            get { return medicaments; }
            set
            {
                medicaments = value;
                OnPropertyChanged(nameof(Medicaments));
            }
        }

        public ObservableCollection<TimeForMedicament> TimeForMedicaments
        {
            get { return timeForMedicaments; }
            set
            {
                timeForMedicaments = value;
                OnPropertyChanged(nameof(TimeForMedicaments));
            }
        }
        public int PerDay
        {
            get { return perDay; }
            set
            {
                perDay = value;
                OnPropertyChanged(nameof(PerDay));
            }
        }

        public Medicament SelectedMedicament
        {
            get { return selectedMedicament; }
            set
            {
                selectedMedicament = value;
                OnPropertyChanged(nameof(SelectedMedicament));
            }
        }

        public TimeForMedicament? SelectedTime
        {
            get { return selectedTime; }
            set
            {
                selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }

        public PrescriptionViewModel(Appointment appointment)
        {
            Appointment = appointment;
            medicaments = new ObservableCollection<Medicament>(Singleton.Instance.MedicamentRepository.Medicaments);
            timeForMedicaments = new ObservableCollection<TimeForMedicament>(Enum.GetValues(typeof(TimeForMedicament)).Cast<TimeForMedicament>().ToList());
        }

        public bool IsValid()
        {
            return perDay > 0 && selectedMedicament != null && selectedTime != null;
        }

        public ICommand PrescriptionCommand
        {
            get { return _prescriptionCommand ??= new PrescriptionCommand(this); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
