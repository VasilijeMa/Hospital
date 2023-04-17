﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    public class Singleton
    {
        private static Singleton instance;
        public Schedule Schedule { get; set; }

        public List<Doctor> doctors;

        public List<Patient> patients;

        public List<Nurse> nurses;

        public List<MedicalRecord> medicalRecords;

        public static Singleton Instance
        {
            get 
            {
                if (instance == null)
                {
                    return instance = new Singleton();
                }
                return instance;
            }
        }
        private Singleton()
        {
            Schedule = new Schedule();
            doctors = Doctor.LoadAll();
            patients = Patient.LoadAll();
            nurses = Nurse.LoadAll();
            medicalRecords = MedicalRecord.LoadAll();
        }
    }
}
