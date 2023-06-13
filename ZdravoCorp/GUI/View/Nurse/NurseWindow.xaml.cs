using System.Windows;
using ZdravoCorp.Core.CommunicationSystem.Services;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.GUI.View.Patient;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for NurseWindow.xaml
    /// </summary>
    public partial class NurseWindow : Window
    {
        Nurse nurse;
        private ChatService chatService;

        public NurseWindow(Nurse nurse, ChatService chatService)
        {
            InitializeComponent();
            this.nurse = nurse;
            this.chatService = chatService;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChatsView chatsView = new ChatsView(nurse, chatService);
            chatsView.ShowDialog();
        }
    }
}
