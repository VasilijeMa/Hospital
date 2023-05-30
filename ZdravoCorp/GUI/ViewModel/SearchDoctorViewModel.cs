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
using ZdravoCorp.Core.Servieces;
using static System.Reflection.Metadata.BlobBuilder;

namespace ZdravoCorp.GUI.ViewModel
{
    public class SearchDoctorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ICommand _searchCommand;
        private ICommand _scheduleCommand;
        private Patient patient;
        private DoctorService doctorService = new DoctorService();
        private ObservableCollection<DoctorListItem> doctorItems;
        private List<Doctor> doctors;
        private DoctorListItem selectedDoctor;
        private String searchBy = "";
        private bool _enable;
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
                UpdateButtonEnabled();
            }
        }
        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                OnPropertyChanged(nameof(Enable));
            }
        }

        private void UpdateButtonEnabled()
        {
            Enable = SelectedDoctor != null;
        }

        public SearchDoctorViewModel(Patient patient)
        {
            this.patient = patient;
            doctors = doctorService.GetDoctors();
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
