using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;
using ZdravoCorp.GUI.PatientHealthcare.Hospitalcare.ViewModel;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Commands
{
    internal class EndHospitalizationCommand : BaseCommand
    {
        private HospitalizedPatientViewModel viewModel;
        private ScheduleService scheduleService = new ScheduleService();
        private PatientService patientService = new PatientService();

        public EndHospitalizationCommand(HospitalizedPatientViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            DialogResult dialogResult = MessageBox.Show("Check-up", "Do you want to schedule a check-up for the patient?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                MakeAppointmentDoctor makeAppointmentDoctor = new MakeAppointmentDoctor(viewModel.doctor);
                DateTime dateForCheckUp = DateTime.Now.AddDays(10);
                makeAppointmentDoctor.dpDate.SelectedDate = dateForCheckUp;
                Appointment appointment = scheduleService.GetAppointmentByExaminationId(viewModel.SelectedExamination.ExaminationId);
                Patient patient = patientService.GetById(appointment.PatientId);
                makeAppointmentDoctor.cmbPatients.SelectedItem = patient;
                makeAppointmentDoctor.ShowDialog();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Hospitalization is done.");
            }
        }
    }
}
