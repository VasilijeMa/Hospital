using System.Windows;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for NurseWindow.xaml
    /// </summary>
    public partial class NurseWindow : Window
    {
        Nurse nurse;
        public NurseWindow(Nurse nurse)
        {
            InitializeComponent();
            this.nurse = nurse;
        }

        private void CRUDpatients_Click(object sender, RoutedEventArgs e)
        {
            CrudPatientWindow crudPW = new CrudPatientWindow(nurse);
            crudPW.ShowDialog();
        }

        private void PatientAdmission_Click(object sender, RoutedEventArgs e)
        {
            PatientAdmissionWindow patientAdmissionWindow = new PatientAdmissionWindow();
            patientAdmissionWindow.ShowDialog();
        }

        private void Emergency_Click(object sender, RoutedEventArgs e)
        {
            EmergencyOperationOrExamination emergencyOperationOrExamination = new EmergencyOperationOrExamination();
            emergencyOperationOrExamination.ShowDialog();
        }

        private void SchedulingAppointment_Click(object sender, RoutedEventArgs e)
        {
            SchedulingAppointmentUsingReferral schedulingAppointmentUsingReferral = new SchedulingAppointmentUsingReferral();
            schedulingAppointmentUsingReferral.ShowDialog();
        }

        private void DepensingMedicaments_Click(object sender, RoutedEventArgs e)
        {
            DepensingMedicaments depensingMedicaments = new DepensingMedicaments();
            depensingMedicaments.ShowDialog();
        }

        private void SchedulingAppointment_Click_1(object sender, RoutedEventArgs e)
        {
            SchedulingAppointment schedulingAppointment = new SchedulingAppointment();
            schedulingAppointment.ShowDialog();
        }

        private void MedicamentsLessThanFive_Click(object sender, RoutedEventArgs e)
        {
            MedicamentsToOrder medicamentsToOrder = new MedicamentsToOrder();
            medicamentsToOrder.ShowDialog();
        }
    }
}
