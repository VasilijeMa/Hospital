using System.Windows;
using System.Windows.Forms;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Model;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Services;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.View;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    public partial class AnamnesisView : Window
    {
        private Appointment selectedAppointment;
        private ConfigRoles role;
        private Anamnesis anamnesis;
        private AnamnesisService anamnesisService;

        public AnamnesisView(Appointment selectedAppointment, AnamnesisService anamnesisService , ConfigRoles role)
        {
            InitializeComponent();
            this.anamnesisService = anamnesisService;
            this.selectedAppointment = selectedAppointment;
            this.role = role;
            setWindow();
        }

        private void setWindow()
        {
            if (role == ConfigRoles.Nurse)
            {
                SetNurseWindow();
            }
            else if (role == ConfigRoles.Doctor)
            {
                SetDoctorWindow();
            }
            else
            {
                SetPatientWindow();
            }
        }

        private void SetPatientWindow()
        {
            Symptoms.IsReadOnly = true;
            DoctorObservation.IsReadOnly = true;
            DoctorConclusion.IsReadOnly = true;
            btnCancel.Visibility = Visibility.Hidden;
            btnSubmit.Visibility = Visibility.Hidden;
            btnChangeEquipment.Visibility = Visibility.Hidden;
            btnHospitalizationRefer.Visibility = Visibility.Hidden;
            btnSpecializationRefer.Visibility = Visibility.Hidden;
            anamnesis = findAnamnesisById(selectedAppointment);
            LoadFields(anamnesis);
        }

        private void SetDoctorWindow()
        {
            this.anamnesis = findAnamnesisById(selectedAppointment);
            Symptoms.Text = anamnesis.Symptoms;
            Symptoms.IsReadOnly = true;
        }

        private void SetNurseWindow()
        {
            DoctorConclusion.IsReadOnly = true;
            DoctorObservation.IsReadOnly = true;
            btnChangeEquipment.Visibility = Visibility.Hidden;
            btnHospitalizationRefer.Visibility = Visibility.Hidden;
            btnSpecializationRefer.Visibility = Visibility.Hidden;
        }

        private void SubmitClick(object sender, RoutedEventArgs e)
        {
            if (isValid())
            {
                if (role == ConfigRoles.Nurse)
                {
                    createAnamnesisObject();
                }
                else
                {
                    anamnesis.DoctorsConclusion = DoctorConclusion.Text;
                    anamnesis.DoctorsObservation = DoctorObservation.Text;
                    anamnesisService.WriteAll();
                }
                MessageBox.Show("You successefully added anamnesis.", "Information", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to quit? ", "Question", MessageBoxButtons.YesNo);
            if (dialogResult.ToString() == "Yes")
            {
                this.Close();
            }
        }

        private void LoadFields(Anamnesis anamnesis)
        {
            Symptoms.Text = anamnesis.Symptoms;
            DoctorObservation.Text = anamnesis.DoctorsObservation.ToString();
            DoctorConclusion.Text = anamnesis.DoctorsConclusion.ToString();
        }

        private bool isValid()
        {
            if (role == ConfigRoles.Nurse)
            {
                return isValidForNurseInput();
            }
            else
            {
                return isValidForDoctorInput();
            }
        }

        private bool isValidForNurseInput()
        {
            if (Symptoms.Text.Length == 0)
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool isValidForDoctorInput()
        {
            if ((DoctorObservation.Text.Length == 0) || (DoctorConclusion.Text.Length == 0))
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public Anamnesis findAnamnesisById(Appointment selectedAppointment)
        {
            foreach (Anamnesis anamnesis in anamnesisService.GetAnamneses())
            {
                if (anamnesis.AppointmentId == selectedAppointment.Id)
                {
                    return anamnesis;
                }
            }
            return null;
        }

        private void createAnamnesisObject()
        {
            Anamnesis anamnesis = new Anamnesis(selectedAppointment.Id,
                                                       selectedAppointment.PatientId,
                                                       Symptoms.Text,
                                                       "",
                                                       ""
                                                       );
            this.anamnesis = anamnesis;
            anamnesisService.AddAnamnesis(anamnesis);
            anamnesisService.WriteAll();
        }

        private void useUpDynamicEquipment(object sender, RoutedEventArgs e)
        {
            EquipmentUsedByDoctor used = new EquipmentUsedByDoctor(selectedAppointment);
            used.ShowDialog();
        }

        private void HospitalizatinRefer_Click(object sender, RoutedEventArgs e)
        {
            HospitalizationReferralView hospitalizationReferralView = new HospitalizationReferralView(selectedAppointment);
            hospitalizationReferralView.ShowDialog();
        }

        private void SpecializationRefer_Click(object sender, RoutedEventArgs e)
        {
            SpecializationReferralView specializationReferralView = new SpecializationReferralView(selectedAppointment);
            specializationReferralView.ShowDialog();
        }
    }
}
