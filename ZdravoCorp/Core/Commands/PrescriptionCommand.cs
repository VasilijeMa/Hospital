using System;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;
using ZdravoCorp.GUI.ViewModel;

namespace ZdravoCorp.Core.Commands
{
    internal class PrescriptionCommand : BaseCommand
    {
        private MedicalRecordService medicalRecordService = new MedicalRecordService();
        private ScheduleService scheduleService = new ScheduleService();

        private NotificationService notificationService;
        //private NotificationRepository notificationRepository = Singleton.Instance.NotificationRkepository;
        private PrescriptionViewModel viewModel;

        public PrescriptionCommand(PrescriptionViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (!viewModel.IsValid())
            {
                MessageBox.Show("You must fill in all fields.");
                return;
            }
            Prescription prescription = CreatePrescription();
            //prescriptionService.AddPrescription(prescription);
            if (medicalRecordService.IsAllergic(viewModel.Appointment.PatientId, viewModel.SelectedMedicament))
            {
                MessageBox.Show("The patient has an allergy to the medicament");
                return ;
            }
            Examination examination =  CreateExamination(prescription);
            MessageBox.Show("You have successfully added a prescription!");
        }

        private Examination CreateExamination(Prescription prescription)
        {
            Examination examination;
            if (viewModel.Appointment.ExaminationId == 0)
            {
                examination = new Examination(prescription);
                Singleton.Instance.ExaminationRepository.Add(examination);
                viewModel.Appointment.ExaminationId = examination.Id;
                scheduleService.WriteAllAppointmens();
            }
            else
            {
                examination = Singleton.Instance.ExaminationRepository.GetExaminationById(viewModel.Appointment.ExaminationId);
                examination.Prescription = prescription;
                Singleton.Instance.ExaminationRepository.WriteAll();
            }
            return examination;
        }

        public Prescription CreatePrescription()
        {
            Medicament medicament = viewModel.SelectedMedicament;
            Instruction instruction = new Instruction(viewModel.PerDay, viewModel.SelectedTime);
            Prescription prescription = new Prescription(medicament, instruction);
            notificationService = new NotificationService(viewModel.Appointment.PatientId);
            notificationService.CreateNotification(medicament.Name, viewModel.Appointment.PatientId, instruction.TimePerDay, 0, null);
            return prescription;
        }
    }
}
