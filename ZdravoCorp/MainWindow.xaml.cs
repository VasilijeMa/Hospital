﻿using System;
using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Core.CommunicationSystem.Services;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.PatientSatisfaction.Services;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.ManagerView;

//using System.Collections;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> users;
        private UserService userService;
        private DoctorService doctorService;
        private PatientService patientService;
        private ChatService chatService;
        private MedicamentsToAddService medicamentsToAddService = new MedicamentsToAddService();
        private DoctorSurveyService doctorSurveyService;
        private HospitalSurveyService hospitalSurveyService;
        public MainWindow()
        {
            InitializeComponent();
            userService = new UserService();
            users = userService.GetUsers();
            SetServices();
        }

        private void SetServices()
        {
            doctorSurveyService = new DoctorSurveyService(Singleton.Instance.DoctorSurveyRepository);
            hospitalSurveyService = new HospitalSurveyService(Singleton.Instance.HospitalSurveyRepository);
            chatService = new ChatService(Singleton.Instance.ChatRepository);
            doctorService = new DoctorService();
            patientService = new PatientService();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var user in users)
            {
                if (tbUsername.Text == user.Username && pbPassword.Password == user.Password)
                {
                    this.Visibility = Visibility.Hidden;
                    medicamentsToAddService.checkOrderedMedicaments();
                    DisplayWindow(user);
                    this.Visibility = Visibility.Visible;
                    tbUsername.Text = "";
                    pbPassword.Password = "";
                    return;
                }
            }
            pbPassword.Password = "";
            MessageBox.Show("Invalid username or password.");
        }

        public void DisplayWindow(User user)
        {
            switch (user.Type)
            {
                case "doctor":
                    OpenDoctorWindow(user);
                    break;
                case "nurse":
                    OpenNurseWindow(user);
                    break;
                case "manager":
                    ManagerWindow managerWindow = new ManagerWindow();
                    managerWindow.ShowDialog();
                    break;
                case "patient":
                    OpenPatientWindow(user);
                    break;
                default:
                    break;
            }
        }

        private void OpenDoctorWindow(User user)
        {
            foreach (Doctor doctor in doctorService.GetDoctors())
            {
                if (user.Username == doctor.Username)
                {
                    DoctorWindow doctorWindow = new DoctorWindow(doctor, chatService);
                    doctorWindow.ShowDialog();
                    break;
                }
            }
        }

        private void OpenNurseWindow(User user)
        {
            foreach (Nurse nurse in Singleton.Instance.NurseRepository.GetNurses())
            {
                if (user.Username == nurse.Username)
                {
                    NurseWindow nurseWindow = new NurseWindow(nurse, chatService);
                    nurseWindow.ShowDialog();
                    break;
                }
            }
        }

        private void OpenPatientWindow(User user)
        {
            Patient patient = patientService.GetByUsername(user.Username);
            if (!patient.IsBlocked)
            {
                PatientWindow patientWindow = new PatientWindow(patient, doctorSurveyService, hospitalSurveyService);
                patientWindow.ShowDialog();
                if (patient.IsBlocked)
                {
                    userService.WriteAll();
                }

                return;
            }

            MessageBox.Show("Your account is blocked.");
        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
