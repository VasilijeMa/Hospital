﻿using System.Windows;
using System.Windows.Forms;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Domain.Enums;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.View;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    public partial class AnamnesisView : Window
    {
        private Appointment selectedAppointment;
        private ConfigRoles role;
        private Anamnesis anamnesis;
        private AnamnesisRepository anamnesisRepository;
        public AnamnesisView(Appointment selectedAppointment, ConfigRoles role)
        {

            InitializeComponent();
            this.selectedAppointment = selectedAppointment;
            this.role = role;
            anamnesisRepository = Singleton.Instance.AnamnesisRepository;
            setWindow();
        }

        private void setWindow()
        {
            if (role == ConfigRoles.Nurse)
            {
                DoctorConclusion.IsReadOnly = true;
                DoctorObservation.IsReadOnly = true;
                btnChangeEquipment.Visibility = Visibility.Hidden;
                btnHospitalizationRefer.Visibility = Visibility.Hidden;
                btnSpecializationRefer.Visibility = Visibility.Hidden;
            }
            else if (role == ConfigRoles.Doctor)
            {
                this.anamnesis = findAnamnesisById(selectedAppointment);
                Symptoms.Text = anamnesis.Symptoms;
                Symptoms.IsReadOnly = true;
            }
            else
            {
                Symptoms.IsReadOnly = true;
                DoctorObservation.IsReadOnly = true;
                DoctorConclusion.IsReadOnly = true;
                btnCancel.Visibility = Visibility.Hidden;
                btnSubmit.Visibility = Visibility.Hidden;
                btnChangeEquipment.Visibility = Visibility.Hidden;
                btnHospitalizationRefer.Visibility = Visibility.Hidden;
                btnSpecializationRefer.Visibility = Visibility.Hidden;
                anamnesis = findAnamnesisById(selectedAppointment);
                LoadFields(anamnesis);
            }
        }

        private void SubmitClick(object sender, RoutedEventArgs e)
        {
            if (isValid())
            {
                if (role == ConfigRoles.Nurse)
                {
                    createAnamnesisObject();
                }
                else
                {
                    anamnesis.DoctorsConclusion = DoctorConclusion.Text;
                    anamnesis.DoctorsObservation = DoctorObservation.Text;
                    anamnesisRepository.WriteAll();
                }
                MessageBox.Show("You successefully added anamnesis.", "Information", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to quit? ", "Question", MessageBoxButtons.YesNo);
            if (dialogResult.ToString() == "Yes")
            {
                this.Close();
            }
        }

        private void LoadFields(Anamnesis anamnesis)
        {
            Symptoms.Text = anamnesis.Symptoms;
            DoctorObservation.Text = anamnesis.DoctorsObservation.ToString();
            DoctorConclusion.Text = anamnesis.DoctorsConclusion.ToString();
        }

        private bool isValid()
        {
            if (role == ConfigRoles.Nurse)
            {
                return isValidForNurseInput();
            }
            else
            {
                return isValidForDoctorInput();
            }
        }

        private bool isValidForNurseInput()
        {
            if (Symptoms.Text.Length == 0)
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool isValidForDoctorInput()
        {
            if ((DoctorObservation.Text.Length == 0) || (DoctorConclusion.Text.Length == 0))
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public Anamnesis findAnamnesisById(Appointment selectedAppointment)
        {
            foreach (Anamnesis anamnesis in anamnesisRepository.Anamneses)
            {
                if (anamnesis.AppointmentId == selectedAppointment.Id)
                {
                    return anamnesis;
                }
            }
            return null;
        }

        private bool isAlreadyExsist(int appointmentId)
        {
            foreach (Anamnesis anamnesis in anamnesisRepository.Anamneses)
            {
                if (anamnesis.AppointmentId == appointmentId)
                {
                    MessageBox.Show("There is already an anamnesis in this appointemnt.");
                    return true;
                }
            }
            return false;
        }

        public Patient getPatient()
        {
            foreach (Patient patient in Singleton.Instance.PatientRepository.Patients)
            {
                if (patient.Id == anamnesis.PatientId)
                {
                    return patient;
                }
            }
            return null;
        }


        /*private void refreshMedicalRecord()
        {
            Patient patient = getPatient();
            MedicalRecord medicalRecord = patient.getMedicalRecord();
            medicalRecord.WriteAll(Singleton.Instance.medicalRecords);
        }*/

        private void createAnamnesisObject()
        {
            Anamnesis anamnesis = new Anamnesis(selectedAppointment.Id,
                                                       selectedAppointment.PatientId,
                                                       Symptoms.Text,
                                                       "",
                                                       ""
                                                       );
            this.anamnesis = anamnesis;
            anamnesisRepository.Anamneses.Add(anamnesis);
            anamnesisRepository.WriteAll();
        }

        private void useUpDynamicEquipment(object sender, RoutedEventArgs e)
        {
            EquipmentUsedByDoctor used = new EquipmentUsedByDoctor(selectedAppointment);
            used.ShowDialog();
        }

        private void HospitalizatinRefer_Click(object sender, RoutedEventArgs e)
        {
            HospitalizationReferralView hospitalizationReferralView = new HospitalizationReferralView();
            hospitalizationReferralView.ShowDialog();
        }

        private void SpecializationRefer_Click(object sender, RoutedEventArgs e)
        {
            SpecializationReferralView specializationReferralView = new SpecializationReferralView(selectedAppointment);
            specializationReferralView.ShowDialog();
        }
    }
}