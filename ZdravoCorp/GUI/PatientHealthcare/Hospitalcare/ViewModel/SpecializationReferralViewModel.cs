﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Commands;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;

namespace ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel
{
    public class SpecializationReferralViewModel : INotifyPropertyChanged
    {
        ExaminationService examinationService = new ExaminationService();
        private DoctorService doctorService = new DoctorService();
        private ICommand _refereCommand;
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
            FillFields();
            doctors = new ObservableCollection<Doctor>(doctorService.GetDoctors());
            specializations = new ObservableCollection<Specialization>(doctorService.GetSpecializationOfDoctors());
        }

        public bool IsValid()
        {
            return isDoctor == false && isSpecialization == false;
        }

        public void FillFields()
        {
            if (Appointment.ExaminationId == 0) return;
            Examination examination = examinationService.GetExaminationById(Appointment.ExaminationId);
            if (examination.SpecializationRefferal != null)
            {
                if (examination.SpecializationRefferal.Specialization != null)
                {
                    isSpecialization = true;
                    SelectedSpecialization = examination.SpecializationRefferal.Specialization;
                    return;
                }
                if (examination.SpecializationRefferal.DoctorId != -1)
                {
                    isDoctor = true;
                    Doctor doctor = doctorService.GetDoctor(examination.SpecializationRefferal.DoctorId);
                    SelectedDoctor = doctor;
                }
            }
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
