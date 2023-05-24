﻿using System.Windows;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.ManagerView;

namespace ZdravoCorp.Core.Domain
{
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Type { get; set; }

        public bool IsBlocked { get; set; }

        public User() { }

        public User(string username, string password, string type)
        {
            Username = username;
            Password = password;
            Type = type;
        }

        public override string ToString()
        {
            return "Username: " + Username + " Password: " + Password + "Type: " + Type;
        }

        public static void DisplayWindow(User user)
        {
            switch (user.Type)
            {
                case "doctor":
                    foreach (Doctor doctor in Singleton.Instance.DoctorRepository.Doctors)
                    {
                        if (user.Username == doctor.Username)
                        {
                            DoctorWindow doctorWindow = new DoctorWindow(doctor);
                            doctorWindow.ShowDialog();
                            break;
                        }
                    }
                    break;

                case "nurse":
                    foreach (Nurse nurse in Singleton.Instance.NurseRepository.Nurses)
                    {
                        if (user.Username == nurse.Username)
                        {
                            NurseWindow nurseWindow = new NurseWindow(nurse);
                            nurseWindow.ShowDialog();
                            break;
                        }
                    }
                    break;

                case "manager":
                    ManagerWindow managerWindow = new ManagerWindow();
                    managerWindow.ShowDialog();
                    break;
                case "patient":
                    foreach (Patient patient in Singleton.Instance.PatientRepository.Patients)
                    {
                        if (user.Username == patient.Username)
                        {
                            if (!patient.IsBlocked)
                            {
                                PatientWindow patientWindow = new PatientWindow(patient);
                                patientWindow.ShowDialog();
                                if (patient.IsBlocked)
                                {
                                    PatientService patientService = new PatientService();
                                    patientService.WriteAll();
                                }
                                break;
                            }
                            MessageBox.Show("Your account is blocked.");
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}