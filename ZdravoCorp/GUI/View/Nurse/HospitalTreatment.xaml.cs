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
        private Examination examination;
        private DateOnly startDate;
        private DateOnly endDate;
        private string roomId;
        private PatientService patientService = new PatientService();
        private ExaminationService examinationService = new ExaminationService();
        private HospitalStayService hospitalStayService = new HospitalStayService();
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
            if (rooms.SelectedItem != null) 
            {
                HospitalStay hospitalStay = new HospitalStay(examination.Id,startDate,endDate,rooms.SelectedItem.ToString());
                hospitalStayService.Add(hospitalStay);
                examination.HospitalizationRefferal.RoomId = rooms.SelectedItem.ToString();
                examinationService.WriteAll();
                MessageBox.Show("You have successfully allocated a room for hospital treatment.", "Information", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
                this.Close();
            }
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
                Examination examination = examinationService.GetExaminationById(int.Parse(patientsExaminations.SelectedItem.ToString()));
                this.examination = examination;
                DateOnly startDate = examination.HospitalizationRefferal.StartDate;
                this.startDate = startDate;
                int duration = examination.HospitalizationRefferal.Duration;
                DateOnly endDate = startDate.AddDays(duration);
                this.endDate = endDate;
                List<string> freeRooms = hospitalStayService.FindFreeRooms(startDate, endDate);
                foreach (var roomName in freeRooms) 
                {
                    rooms.Items.Add(roomName);
                }
        }
        private void showReferrals_Click(object sender, RoutedEventArgs e)
        {
            if (patients.SelectedItem != null)
            {
                List<int> examinationsIds = examinationService.GetExaminationsIdsForHospitalizationReferral(patients.SelectedItem.ToString());
                if (examinationsIds.Count() != 0)
                {
                    patientsExaminations.Items.Clear();
                    fillReferralsData(examinationsIds);
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

        private void showRooms_Click(object sender, RoutedEventArgs e)
        {
            if (patientsExaminations.SelectedItem != null)
            {
                fillRoomsData();
            }
            else 
            {
                MessageBox.Show("You must select examination id which hospitalization referral you want to use.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
            }
        }
    }
}
