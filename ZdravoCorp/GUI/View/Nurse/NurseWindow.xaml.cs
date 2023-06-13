using System.Windows;
using ZdravoCorp.Core.CommunicationSystem.Services;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;
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
        private AnamnesisService anamnesisService;
        private HospitalStayService hospitalStayService;

        public NurseWindow(Nurse nurse, ChatService chatService, AnamnesisService anamnesisService, HospitalStayService hospitalStayService)
        {
            InitializeComponent();
            this.nurse = nurse;
            this.chatService = chatService;
            this.anamnesisService = anamnesisService;
            this.hospitalStayService = hospitalStayService;
        }

        private void CRUDpatients_Click(object sender, RoutedEventArgs e)
        {
            CrudPatientWindow crudPW = new CrudPatientWindow(nurse);
            crudPW.ShowDialog();
        }

        private void PatientAdmission_Click(object sender, RoutedEventArgs e)
        {
            PatientAdmissionWindow patientAdmissionWindow = new PatientAdmissionWindow(anamnesisService);
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

        private void HospitalTreatmentForPatient_Click(object sender, RoutedEventArgs e)
        {
            HospitalTreatment hospitalTreatment = new HospitalTreatment(hospitalStayService);
            hospitalTreatment.ShowDialog();
        }
    }
}
