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
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorp.GUI.ViewModel
{
    public class HospitalizationReferralViewModel:INotifyPropertyChanged
    {
        ExaminationService examinationService = new ExaminationService();
        private MedicamentService _medicamentService = new MedicamentService();
        private ICommand _submitCommand;
        public ObservableCollection<Medicament> medicaments;
        public ObservableCollection<TimeForMedicament> timeForMedicaments;
        private Medicament selectedMedicament = null;
        public TimeForMedicament? selectedTime = null;
        public Appointment Appointment { get; set; }

        private int duration;
        private int perDay;
        private string testing="";
        private int durationForMedicament;

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
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }
        
        public int DurationForMedicament
        {
            get { return durationForMedicament; }
            set
            {
                durationForMedicament = value;
                OnPropertyChanged(nameof(DurationForMedicament));
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
        public string Testing
        {
            get { return testing; }
            set
            {
                testing = value;
                OnPropertyChanged(nameof(Testing));
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

        public HospitalizationReferralViewModel(Appointment appointment)
        {
            Appointment = appointment;
            FillFields();
            medicaments = new ObservableCollection<Medicament>(_medicamentService.GetMedicaments());
            timeForMedicaments = new ObservableCollection<TimeForMedicament>(Enum.GetValues(typeof(TimeForMedicament)).Cast<TimeForMedicament>().ToList());
        }

        public void FillFields()
        {
            if (Appointment.ExaminationId == 0) return;
            Examination examination = examinationService.GetExaminationById(Appointment.ExaminationId);
            if (examination.HospitalizationRefferal != null)
            {
                PerDay = examination.HospitalizationRefferal.InitialTherapy.Instruction.TimePerDay;
                Duration = examination.HospitalizationRefferal.Duration;
                Testing = examination.HospitalizationRefferal.AdditionalTesting;
                SelectedTime = examination.HospitalizationRefferal.InitialTherapy.Instruction.TimeForMedicament;
                Medicament medicament =
                    _medicamentService.GetMedicamentById(examination.HospitalizationRefferal.InitialTherapy.Medicament
                        .Id);
                SelectedMedicament = medicament;
                DurationForMedicament = examination.HospitalizationRefferal.InitialTherapy.Instruction.Duration;
            }
        }

        public bool IsValid()
        {
            return perDay > 0 && selectedMedicament != null && selectedTime != null && duration > 0;
        }

        public ICommand SubmitCommand
        {
            get { return _submitCommand ??= new HospitalizationReferralCommand(this); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
