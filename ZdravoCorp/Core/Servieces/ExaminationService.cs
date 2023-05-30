﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;
using MessageBox = System.Windows.Forms.MessageBox;
using System.IO;

namespace ZdravoCorp.Core.Servieces
{
    public class ExaminationService
    {
        private IExaminationRepository examinationRepository;
        private ScheduleService scheduleService;
        private ScheduleRepository scheduleRepository;
        private PatientService patientService;
        public ExaminationService()
        {
            this.examinationRepository = Singleton.Instance.ExaminationRepository;
            this.scheduleService = new ScheduleService();
            this.patientService = new PatientService();
            this.scheduleRepository = new ScheduleRepository();
        }

        public void WriteAll()
        {
            examinationRepository.WriteAll();
        }

        public void Add(Examination examination)
        {
            examinationRepository.Add(examination);
        }

        public Examination GetExaminationById(int examinationId)
        {
            return examinationRepository.GetExaminationById(examinationId);
        }

        public List<int> GetExaminationsIdsForPrescriptions(String patientUsername) 
        {
            List<int> examinationsIds = new List<int>();
            Patient patient = patientService.GetByUsername(patientUsername);
            foreach (Appointment appointment in scheduleService.GetAppointmentsForPatient(patient.Id))
            {
                if (appointment.ExaminationId != 0)
                {
                    Examination patientsExamination = GetExaminationById(appointment.ExaminationId);
                    if (patientsExamination != null)
                    {
                        if (patientsExamination.Prescription != null)
                        {
                            if (patientsExamination.Prescription.IsUsed == false)
                            {
                                examinationsIds.Add(patientsExamination.Id);
                            }
                        }
                    }
                }
            }
            return examinationsIds;
        }

        public List<Examination> GetExaminationsByMedicamentId(int medicamentId) 
        {
            List<Examination> examinations = new List<Examination>();
            foreach (Appointment appointment in scheduleRepository.GetAppointments()) 
            {
                if (appointment.ExaminationId != 0) 
                {
                    Examination examination = GetExaminationById(appointment.ExaminationId);
                    if (examination != null)
                    {
                        if (examination.Prescription != null)
                        {
                            if (examination.Prescription.Medicament.Id == medicamentId)
                            {
                                examinations.Add(examination);
                            }
                        }
                    }
                }
            }
            return examinations;
        }
        public List<int> GetExaminationsIdsForReferral(String patientUsername) 
        {
            List<int> examinationsIds = new List<int>();
            Patient patient = patientService.GetByUsername(patientUsername);
            foreach (Appointment appointment in scheduleService.GetAppointmentsForPatient(patient.Id))
            {
                if (appointment.ExaminationId != 0)
                {
                    Examination patientsExamination = GetExaminationById(appointment.ExaminationId);
                    if (patientsExamination != null)
                    {
                        if (patientsExamination.SpecializationRefferal != null)
                            {
                            if (patientsExamination.SpecializationRefferal.IsUsed == false)
                            {
                                examinationsIds.Add(patientsExamination.Id);
                            } 
                        }
                    }
                }
            }
            return examinationsIds;
        }
    }
}
