using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Commands;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model;
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorp.GUI.ViewModel
{
    public class HospitalTreatmentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ICommand _showReferralsCommand;
        private ICommand _showRoomsCommand;
        private ICommand _confirmCommand;
        private ObservableCollection<Patient> patients;
        private ObservableCollection<string> roomIds;
        private ObservableCollection<int> examinationIds;
        private Patient patientUsername;
        private string roomId;
        private int examinationId;
        private PatientService patientService = new PatientService();
        private ExaminationService examinationService = new ExaminationService();
        public HospitalStayService hospitalStayService;
        
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Examination Examination { get; set; }

        public HospitalTreatmentViewModel(HospitalStayService hospitalStayService)
        {
            this.hospitalStayService = hospitalStayService;
            Patients = new ObservableCollection<Patient> (patientService.GetPatients());
        }
        public ICommand ShowReferralsCommand
        {
            get { return _showReferralsCommand ??= new ShowReferralsCommand(this); }
        }

        public ICommand ShowRoomsCommand
        {
            get { return _showRoomsCommand ??= new ShowRoomsCommand(this); }
        }

        public ICommand ConfirmCommand
        {
            get { return _confirmCommand ??= new ConfirmCommand(this); }
        }

        public Patient PatientUsername
        {
            get { return patientUsername; }
            set
            {
                patientUsername = value;
                OnPropertyChanged(nameof(PatientUsername));
                ((BaseCommand)ShowReferralsCommand).RaiseCanExecuteChanged();
            }
        }

        public string RoomId
        {
            get { return roomId; }
            set
            {
                roomId = value;
                OnPropertyChanged(nameof(roomId));
                ((BaseCommand)ConfirmCommand).RaiseCanExecuteChanged();
            }
        }
        public int ExaminationId
        {
            get { return examinationId; }
            set
            {
                examinationId = value;
                OnPropertyChanged(nameof(ExaminationId));
                ((BaseCommand)ShowRoomsCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Patient> Patients
        {
            get { return patients; }
            set
            {
                patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }
        public ObservableCollection<string> Rooms
        {
            get { return roomIds; }
            set
            {
                roomIds = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }
        public ObservableCollection<int> Examinations
        {
            get { return examinationIds; }
            set
            {
                examinationIds = value;
                OnPropertyChanged(nameof(Examinations));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
