﻿using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Model;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Services;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;
using ZdravoCorp.GUI.View.Doctor;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{

    public partial class CreateMedicalRecordWindow : Window
    {
        bool doctor;
        bool create;
        bool update;
        Patient patient;
        MedicalRecord selectedRecord;
        Appointment selectedAppointment;
        private MedicalRecordService medicalRecordService;
        private PatientService patientService;
        private UserService userService;
        private AnamnesisService anamnesisService;

        public CreateMedicalRecordWindow(bool create, Patient patient, bool doctor, AnamnesisService anamnesisService, Appointment selectedAppointment = null, bool update = false)
        {
            InitializeComponent();
            medicalRecordService = new MedicalRecordService();
            patientService = new PatientService();
            userService = new UserService();
            this.anamnesisService = anamnesisService;
            this.doctor = doctor;
            this.create = create;
            this.patient = patient;
            this.update = update;
            this.selectedAppointment = selectedAppointment;
            addPrescription.Visibility = Visibility.Hidden;
            setWindow();
        }

        public void setWindow()
        {
            if (doctor)
            {
                addAnamnesis.Visibility = Visibility.Hidden;
                confirm.Visibility = Visibility.Hidden;
                cancel.Visibility = Visibility.Hidden;
                addPrescription.Visibility = Visibility.Visible;
            }
            if (!create)
            {
                addAnamnesis.Visibility = Visibility.Visible;
                confirm.Visibility = Visibility.Visible;
                cancel.Visibility = Visibility.Visible;
                LoadFields();
            }
            if (update)
            {
                addAnamnesis.Visibility = Visibility.Hidden;
            }
        }
        private void LoadFields()
        {
            this.selectedRecord = medicalRecordService.GetMedicalRecordById(patient.MedicalRecordId);
            height.Text = selectedRecord.Height.ToString();
            weight.Text = selectedRecord.Weight.ToString();
            foreach (string oneAnamnesis in selectedRecord.EarlierIllnesses)
            {
                earlyIlnness.Text += oneAnamnesis + ", ";
            }
            earlyIlnness.Text = earlyIlnness.Text.Substring(0, earlyIlnness.Text.Length - 2);
            foreach (string oneAllergen in selectedRecord.Allergens)
            {
                alergy.Text += oneAllergen + ", ";
            }
            alergy.Text = alergy.Text.Substring(0, alergy.Text.Length - 2);
        }
        private void addEarlyIllness(object sender, RoutedEventArgs e)
        {
            string inputIllness = inputDialogT("illness");
            if (inputIllness == "")
            {
                return;
            }
            selectedRecord.EarlierIllnesses.Add(inputIllness);
            earlyIlnness.Text += ", " + inputIllness;
        }

        private void addAlergyClick(object sender, RoutedEventArgs e)
        {
            string inputAlergy = inputDialogT("alergy");
            if (inputAlergy == "")
            {
                return;
            }
            selectedRecord.Allergens.Add(inputAlergy);
            alergy.Text += ", " + inputAlergy;
        }

        private string inputDialogT(string type)
        {
            string input = Interaction.InputBox("Please enter " + type + ": ", "Input dialog");
            if (input == "") return "";
            if (type == "alergy")
            {
                foreach (string alergy in selectedRecord.Allergens)
                {
                    if (alergy == input)
                    {
                        MessageBox.Show(type + " already exists.");
                        return "";
                    }
                }
            }
            else
            {
                foreach (string ilnness in selectedRecord.EarlierIllnesses)
                {
                    if (ilnness == input)
                    {
                        MessageBox.Show(type + " already exists.");
                        return "";
                    }
                }
            }
            return input;
        }

        private bool isValid()
        {
            if ((height.Text.Length == 0) || (weight.Text.Length == 0) || (earlyIlnness.Text.Length == 0))
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            if (!(isDouble(height.Text) && isDouble(weight.Text)))
            {
                MessageBox.Show("Weight and height should be numbers.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void clearData()
        {
            height.Clear();
            weight.Clear();
            earlyIlnness.Clear();
        }

        public void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to quit? ", "Question", MessageBoxButtons.YesNo);
            if (dialogResult.ToString() == "Yes")
            {
                clearData();
                this.Close();
            }
        }

        public void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (isValid())
            {
                MedicalRecord medicalRecord = createMedicalRecordObject();
                if (medicalRecord != null)
                {
                    addToMedicalRecords(medicalRecord);
                }
                addToPatients(patient);
                addToUsers(patient);
                MessageBox.Show("Operation successful. We saved your changes.", "Done", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
                this.Close();
            }
        }

        public MedicalRecord createMedicalRecordObject()
        {
            if (!create)
            {
                setAttributes(selectedRecord);
                medicalRecordService.WriteAll();
                return null;
            }
            MedicalRecord newMedicalRecord = new MedicalRecord();
            setAttributes(newMedicalRecord);
            newMedicalRecord.Id = patient.MedicalRecordId;
            return newMedicalRecord;
        }

        public void setAttributes(MedicalRecord medicalRecord)
        {
            medicalRecord.Height = double.Parse(height.Text);
            medicalRecord.Weight = double.Parse(weight.Text);
            medicalRecord.EarlierIllnesses = earlyIlnness.Text.Split(", ").ToList();
            medicalRecord.Allergens = alergy.Text.Split(", ").ToList();
        }

        public void addToPatients(Patient newPatient)
        {
            if (!create)
            {
                patientService.RemovePatient(patient);
                patientService.WriteAll();
                userService.RemoveUser(patient.Username);
                userService.WriteAll();
            }
            patientService.AddPatient(newPatient);
            patientService.WriteAll();
        }

        public void addToMedicalRecords(MedicalRecord newMedicalRecord)
        {
            medicalRecordService.AddMedicalRecord(newMedicalRecord);
            medicalRecordService.WriteAll();
        }

        public void addToUsers(Patient newPatient)
        {
            userService.AddUser(new User(newPatient.Username, newPatient.Password, "patient"));
            userService.WriteAll();
        }

        public void addAnamnesisClick(object sender, RoutedEventArgs e)
        {
            AnamnesisView anamnesis;
            if (doctor)
            {
                Anamnesis findAnamnesis = anamnesisService.findAnamnesisById(selectedAppointment);
                if (findAnamnesis == null)
                {
                    MessageBox.Show("The patient must first check in with the nurse.");
                    return;
                }
                anamnesis = new AnamnesisView(selectedAppointment, anamnesisService, ConfigRoles.Doctor);
            }
            else
            {
                anamnesis = new AnamnesisView(selectedAppointment, anamnesisService, ConfigRoles.Nurse);
            }
            anamnesis.ShowDialog();
        }

        public bool isDouble(string data)
        {
            double d;
            if (Double.TryParse(data, out d))
            {
                return true;
            }
            return false;
        }

        private void addPrescription_Click(object sender, RoutedEventArgs e)
        {
            if (!CanAddPrescription())
            {
                return;
            }
            PrescriptionView prescriptionView = new PrescriptionView(selectedAppointment);
            prescriptionView.ShowDialog();
        }

        private bool CanAddPrescription()
        {
            Anamnesis findAnamnesis = anamnesisService.findAnamnesisById(selectedAppointment);
            if (findAnamnesis == null)
            {
                MessageBox.Show("The patient must first check in with the nurse.");
                return false;
            }
            if (findAnamnesis.DoctorsObservation == "" && findAnamnesis.DoctorsConclusion == "")
            {
                MessageBox.Show("You must first fill anamnesis");
                return false;
            }
            return true;
        }
    }
}
