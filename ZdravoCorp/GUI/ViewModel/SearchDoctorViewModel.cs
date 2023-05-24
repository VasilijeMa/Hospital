using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using static System.Reflection.Metadata.BlobBuilder;

namespace ZdravoCorp.GUI.ViewModel
{
    public class SearchDoctorViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ICommand _searchCommand;
        private ICommand _scheduleCommand;
        private Patient patient;
        private DoctorRepository doctorRepository;
        private ObservableCollection<DoctorListItem> doctorItems;
        private List<Doctor> doctors;
        private DoctorListItem selectedDoctor;
        private String searchBy = "";
        public String SearchBy
        {
            get { return searchBy; }
            set
            {
                searchBy = value;
                OnPropertyChanged(nameof(SearchBy));
            }
        }
        public ObservableCollection<DoctorListItem> DoctorItems
        {
            get { return doctorItems; }
            set
            {
                doctorItems = value;
                OnPropertyChanged(nameof(DoctorItems));
            }
        }

        public DoctorListItem SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                OnPropertyChanged();
            }
        }
        public SearchDoctorViewModel(Patient patient)
        {
            this.patient = patient;
            doctorRepository = new DoctorRepository();
            doctors = doctorRepository.Doctors;
            DoctorItems = new ObservableCollection<DoctorListItem>();
            FillData(doctors);
        }
        public ICommand SearchCommand
        {
            get { return _searchCommand ??= new SearchDoctorCommand(this); }
        }

        public ICommand ScheduleCommand
        {
            get { return _scheduleCommand ??= new ScheduleAppointmentCommand(this, patient); }
        }
        public void FillData(List<Doctor> doctors)
        {
            DoctorItems.Clear();
            foreach (var doctor in doctors)
            {
                DoctorItems.Add(new DoctorListItem(doctor));
            }
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
