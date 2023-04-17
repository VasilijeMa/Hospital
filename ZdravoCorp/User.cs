﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ZdravoCorp
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
        public static List<User> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/users.json");
            var json = reader.ReadToEnd();
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            return users;
        }
        public static void WriteAll(List<User> users)
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("./../../../data/users.json", json);
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
                    Doctor doctor = new Doctor();
             
                    foreach (Doctor oneDoctor in Singleton.Instance.doctors)
                    {
                        if (user.Username == oneDoctor.Username)
                        {
                            doctor = oneDoctor;
                        }
                    }

                    DoctorWindow doctorWindow = new DoctorWindow(doctor);
                    doctorWindow.ShowDialog();
                    break;

                case "nurse":
                    foreach (Nurse nurse in Singleton.Instance.nurses) {
                        if (user.Username == nurse.Username) {
                            NurseWindow nurseWindow = new NurseWindow(nurse);
                            nurseWindow.Show();
                            break;
                        }
                    }
                    break;

                case "manager":

                    break;
                case "patient":
                    foreach (Patient patient in Singleton.Instance.patients)
                    {
                        if (user.Username == patient.Username )
                        {
                            if (!patient.IsBlocked)
                            {
                                PatientWindow patientWindow = new PatientWindow(patient);
                                patientWindow.ShowDialog();
                                if (patient.IsBlocked)
                                {
                                    patient.WriteAll(Singleton.Instance.patients);
                                }
                                break;
                            }
                            MessageBox.Show("Your account is blocked.");
                        }
                    }
                    break;
                default:
                    // code block
                    break;
            }
        }
    }
}