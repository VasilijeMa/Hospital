using System.Windows;
using ZdravoCorp.Domain;

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

    }
}
