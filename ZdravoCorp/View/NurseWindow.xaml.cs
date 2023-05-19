using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
