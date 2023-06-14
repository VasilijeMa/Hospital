using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using System.IO;
using ZdravoCorp.Core.Scheduling.Repositories;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Repositories.Interfaces;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;
using ZdravoCorp.Core.UserManager.Services;
using ZdravoCorp.Core.UserManager.Model;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services
{
    public class ExaminationService
    {
        private IExaminationRepository examinationRepository;
        private ScheduleService scheduleService;
        private ScheduleRepository scheduleRepository;
        private PatientService patientService;
        public ExaminationService()
        {
            examinationRepository = Institution.Instance.ExaminationRepository;
            scheduleService = new ScheduleService();
            patientService = new PatientService();
            scheduleRepository = new ScheduleRepository();
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

        public List<Examination> ExaminationOfHospitalizedPatients(int doctorId)
        {
            List<Examination> exainations = new List<Examination>();
            foreach (var examination in examinationRepository.GetExaminations())
            {
                if (examination.HospitalizationRefferal == null) continue;
                HospitalizationReferral referral = examination.HospitalizationRefferal;
                DateOnly endHospitalizationReferral = referral.StartDate.AddDays(referral.Duration);
                if (!(referral.StartDate <= DateOnly.FromDateTime(DateTime.Now) && DateOnly.FromDateTime(DateTime.Now) <= endHospitalizationReferral)) continue;
                Appointment appointment = scheduleRepository.GetAppointmentByExaminationId(examination.Id);
                Patient hospitalizedPatient = patientService.GetById(appointment.PatientId);
                if (patientService.AlreadyExaminedPatients(doctorId).Contains(hospitalizedPatient))
                {
                    if (!exainations.Contains(examination)) exainations.Add(examination);
                }
            }
            return exainations;
        }

        public List<int> GetExaminationsIdsForPrescriptions(string patientUsername)
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

        public List<int> GetExaminationsIdsForHospitalizationReferral(string patientUsername)
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
                        if (patientsExamination.HospitalizationRefferal != null)
                        {
                            if (patientsExamination.HospitalizationRefferal.RoomId.Equals(""))
                            {
                                examinationsIds.Add(patientsExamination.Id);
                            }
                        }
                    }
                }
            }
            return examinationsIds;
        }
        public List<int> GetExaminationsIdsForReferral(string patientUsername)
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

        public void EndHospitaliztionRefferal(int id)
        {
            Examination examination = GetExaminationById(id);
            examination.HospitalizationRefferal.IsOver = true;
            WriteAll();
        }
    }
}
