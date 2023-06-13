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
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Services;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Model;
using ZdravoCorp.Core.PatientHealthcare.Pharmacy.Services;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MedicamentsToOrder.xaml
    /// </summary>
    public partial class MedicamentsToOrder : Window
    {
        private MedicamentService medicamentService = new MedicamentService();
        private ExaminationService examinationService = new ExaminationService();
        private MedicamentsToAddService medicamentsToAddService = new MedicamentsToAddService();
        public MedicamentsToOrder()
        {
            InitializeComponent();
            fillComboBoxWithMedicaments();
        }

        private void fillComboBoxWithMedicaments() 
        {
            foreach (Medicament medicament in medicamentService.GetMedicaments()) 
            {
                if (medicament.Quantity <= 5) 
                {
                    medicamentsToOrder.Items.Add(medicament.Name);
                }
            }
        }

        private bool MedicamentIsSelected() 
        {
            if (medicamentsToOrder.SelectedItem == null) 
            {
                MessageBox.Show("You must select medicament name.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return false;
            }
            return true;
        }


        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (MedicamentIsSelected() && quantity.Text != "") 
            {
                Medicament medicament = medicamentService.GetByName(medicamentsToOrder.SelectedItem.ToString());
                int newQuantity = medicament.Quantity + int.Parse(quantity.Text);
                MedicamentToAdd medicamentToAdd = new MedicamentToAdd(medicament.Id, newQuantity, DateOnly.FromDateTime(DateTime.Now), false);
                medicamentsToAddService.GetMedicaments().Add(medicamentToAdd);
                medicamentsToAddService.WriteAll();
                MessageBox.Show("Medicament ordered successfully.");
                this.Close();
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
