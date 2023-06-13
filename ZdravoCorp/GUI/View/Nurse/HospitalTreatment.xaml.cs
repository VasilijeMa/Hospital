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
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Servieces;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for HospitalTreatment.xaml
    /// </summary>
    public partial class HospitalTreatment : Window
    {
        private SpecializationReferralService specializationReferralService = new SpecializationReferralService();
        private PatientService patientService = new PatientService();
        private ExaminationService examinationService = new ExaminationService();
        private ScheduleService scheduleService = new ScheduleService();
        private DoctorService doctorService = new DoctorService();
        public HospitalTreatment()
        {
            InitializeComponent();
            fillPatientData();
        }
        private void fillPatientData() 
        {
            foreach (Patient patient in patientService.GetPatients())
            {
                patients.Items.Add(patient.Username);
            }
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to quit? ", "Question", MessageBoxButtons.YesNo);
            if (dialogResult.ToString() == "Yes")
            {
                this.Close();
            }
        }

        private void fillReferralsData(List<int> examinationsIds) 
        {
            foreach (int id in examinationsIds) 
            {
                patientsExaminations.Items.Add(id);
            }
        }

        private void fillRoomsData() 
        {
            if (patientsExaminations.SelectedItem != null)
            {
                Examination examination = examinationService.GetExaminationById(int.Parse(patientsExaminations.SelectedItem.ToString()));
                DateOnly startDate = examination.HospitalizationRefferal.StartDate;
                int duration = examination.HospitalizationRefferal.Duration;
                DateOnly endDate = startDate.AddDays(duration);
                
            }
            else
            {
                MessageBox.Show("You must select the examination id which hospitalization referral you want to use.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
            }
        }
        private void showReferrals_Click(object sender, RoutedEventArgs e)
        {
            if (patients.SelectedItem != null)
            {
                List<int> examinationsIds = examinationService.GetExaminationsIdsForHospitalizationReferral(patients.SelectedItem.ToString());
                if (examinationsIds.Count() != 0)
                {
                    fillReferralsData(examinationsIds);
                    fillRoomsData();
                }
                else
                {
                    MessageBox.Show("This patient doesn't have hospitalization referrals.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                }
            }
            else 
            {
                MessageBox.Show("You must select the patient.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
            }
        }
    }
}
