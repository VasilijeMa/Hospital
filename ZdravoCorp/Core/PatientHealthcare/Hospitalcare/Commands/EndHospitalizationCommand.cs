using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services;
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
        private ExaminationService examinationService = new ExaminationService();

        public EndHospitalizationCommand(HospitalizedPatientViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            if (!viewModel.IsValid())
            {
                MessageBox.Show("Hositalization s already over");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Do you want to schedule an examination?", "End hospitalization", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                MakeAppointment();
            }
            examinationService.EndHospitaliztionRefferal(viewModel.SelectedExamination.ExaminationId);
            MessageBox.Show("Hospitalization is done.");
        }

        public void MakeAppointment()
        {
            MakeAppointmentDoctor makeAppointmentDoctor = new MakeAppointmentDoctor(viewModel.doctor);
            FillInFields(makeAppointmentDoctor);
            makeAppointmentDoctor.ShowDialog();
        }

        private void FillInFields(MakeAppointmentDoctor makeAppointmentDoctor)
        {
            DateTime dateForCheckUp = DateTime.Now.AddDays(10);
            makeAppointmentDoctor.dpDate.SelectedDate = dateForCheckUp;
            Appointment appointment = scheduleService.GetAppointmentByExaminationId(viewModel.SelectedExamination.ExaminationId);
            Patient patient = patientService.GetById(appointment.PatientId);
            makeAppointmentDoctor.cmbPatients.SelectedItem = patient;
        }

    }
}
