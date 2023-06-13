using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Commands;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel
{
    public class HospitalizedPatientViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ExaminationListItem> examinationItems;
        private List<Examination> patients;
        private ExaminationService examinationService = new ExaminationService();
        private ICommand showReferralCommand;
        private ICommand endHospitalizationCommand;
        private ExaminationListItem selectedExamination;
        public Doctor doctor;


        public ExaminationListItem SelectedExamination
        {
            get { return selectedExamination; }
            set
            {
                selectedExamination = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ExaminationListItem> ExaminationItems
        {
            get { return examinationItems; }
            set
            {
                examinationItems = value;
                OnPropertyChanged(nameof(ExaminationItems));
            }
        }

        public HospitalizedPatientViewModel(Doctor doctor)
        {
            this.doctor = doctor;
            patients = examinationService.ExaminationOfHospitalizedPatients(doctor.Id);
            ExaminationItems = new ObservableCollection<ExaminationListItem>();
            FillData(patients);
        }

        public void FillData(List<Examination> patients)
        {
            ExaminationItems.Clear();
            foreach (var patient in patients)
            {
                ExaminationItems.Add(new ExaminationListItem(patient));
            }
        }

        public ICommand EndHospitalizationCommand
        {
            get { return endHospitalizationCommand ??= new EndHospitalizationCommand(this); }
        }

        public ICommand ShowReferralCommand
        {
            get { return showReferralCommand ??= new ShowHospitalizationReferralCommand(this); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
