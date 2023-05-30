using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for DepensingMedicaments.xaml
    /// </summary>
    public partial class DepensingMedicaments : Window
    {

        private PatientService patientService = new PatientService();
        private ExaminationService examinationService = new ExaminationService();
        private MedicamentService medicamentService = new MedicamentService();  
        public DepensingMedicaments()
        {
            InitializeComponent();
            fillComboBoxWithPatients();
        }

        private void showPrescriptions_Click(object sender, RoutedEventArgs e)
        {
            if (isSelectedPatient())
            {
                patientsExaminations.Items.Clear();
                List<int> examinationsIds = examinationService.GetExaminationsIdsForPrescriptions(patients.SelectedItem.ToString());
                if (examinationsIds.Count() != 0)
                {
                    fillComboBoxWithExaminations(examinationsIds);
                }
                else
                {
                    MessageBox.Show("This patient doesn't have any examination.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                }
            }
        }

            private void fillComboBoxWithExaminations(List<int> examinationIds)
            {
                foreach (int id in examinationIds)
                {
                    patientsExaminations.Items.Add(id);
                }
            }


            private bool isSelectedPatient()
        {
            if (patients.SelectedItem == null)
            {
                MessageBox.Show("You must select the patient.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void fillComboBoxWithPatients()
        {
            foreach (Patient patient in patientService.GetPatients())
            {
                patients.Items.Add(patient.Username);
            }
        }

        private bool isSelectedExamination()
        {
            if (patientsExaminations.SelectedItem == null)
            {
                MessageBox.Show("You must select examination id.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (isSelectedExamination()) 
            {
                Examination examination = examinationService.GetExaminationById(int.Parse(patientsExaminations.SelectedItem.ToString()));
                if (medicamentService.IsAvailableThatQuantity(examination.Prescription))
                {
                    if (medicamentService.IsOnTime(examination.Prescription))
                    {
                        examination.Prescription.IsUsed = true;
                        Medicament medicament = medicamentService.GetByPrescription(examination.Prescription);
                        int oldQuantity = medicament.Quantity;
                        medicament.Quantity = oldQuantity - medicamentService.GetRequiredQuantity(examination.Prescription);
                        examination.Prescription.Medicament.Quantity = medicament.Quantity;
                        examinationService.WriteAll();
                        medicamentService.WriteAll();
                        MessageBox.Show("The patient received prescription medication.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("It's not time to get new dose of medicaments.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);

                    }
                }
                else
                { 
                    MessageBox.Show("There is no required quantity for selected prescription.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                }
            }
        }
    }
}
