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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    public partial class AnamnesisView : Window
    {
        private Appointment selectedAppointment;
        private ConfigRoles role;
        private Anamnesis anamnesis;
        public AnamnesisView(Appointment selectedAppointment,ConfigRoles role)
        {

            InitializeComponent();
            this.selectedAppointment = selectedAppointment;
            this.role = role;
            setWindow(role);
        }

        private void setWindow()
        {
            if (role == ConfigRoles.Nurse)
            {
                DoctorConclusion.IsReadOnly = true;
                DoctorObservation.IsReadOnly = true;
            }
            else if (role == ConfigRoles.Doctor)
            {
                Symptoms.Text = anamnesis.Symptoms;
                Symptoms.IsReadOnly = true;
            }
            else
            {
                Symptoms.IsReadOnly = true;
                DoctorObservation.IsReadOnly= true;
                DoctorConclusion.IsReadOnly = true;
                btnCancel.Visibility = Visibility.Hidden;
                btnSubmit.Visibility = Visibility.Hidden;
                anamnesis = findAnamnesisById(selectedAppointment);
                LoadFields(anamnesis);
            }
        }

        private void SubmitClick(object sender, RoutedEventArgs e)
        {
            /*if (isAlreadyExsist(selectedAppointment.Id))
            {
                MessageBox.Show("USLO");
                return;
            }*/

            if (isValid()) {
                if (role == ConfigRoles.Nurse)
                {
                    createAnamnesisObject();
                    MessageBox.Show("You successefully added anamnesis.", "Information", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
                }
                else {
                    //Singleton.Instance.anamnesis.Remove(anamnesis);
                    //anamnesis.WriteAll(Singleton.Instance.anamnesis);
                    anamnesis.DoctorsConclusion = DoctorConclusion.Text;
                    anamnesis.DoctorsObservation = DoctorObservation.Text;
                    //Singleton.Instance.anamnesis.Add(anamnesis);
                    //anamnesis.WriteAll(Singleton.Instance.anamnesis);
                    anamnesis.WriteAll(Singleton.Instance.anamnesis);
                    MessageBox.Show("You successefully added anamnesis.", "Information", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Information);
                }
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

        private void LoadFields(Anamnesis anamnesis) {
            Symptoms.Text = anamnesis.Symptoms;
            DoctorObservation.Text = anamnesis.DoctorsObservation.ToString();
            DoctorConclusion.Text = anamnesis.DoctorsConclusion.ToString();
        }

        private bool isValid() {
            if (isNurse)
            {
                return isValidForNurseInput();
            }
            else {
                return isValidForDoctorInput();
            }
        }

        private bool isValidForNurseInput() {
            if (Symptoms.Text.Length == 0)
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool isValidForDoctorInput() {
            if ((DoctorObservation.Text.Length == 0) || (DoctorConclusion.Text.Length == 0))
            {
                MessageBox.Show("You cannot leave the field blank.", "Failed", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public Anamnesis findAnamnesisById(Appointment selectedAppointment) {
            foreach(Anamnesis anamnesis in Singleton.Instance.anamnesis){
                if (anamnesis.AppointmentId == selectedAppointment.Id) {
                    return anamnesis;
                }
            }
            return null;
        }

        private bool isAlreadyExsist(int appointmentId)
        {
            foreach (Anamnesis anamnesis in Singleton.Instance.anamnesis)
            {
                if (anamnesis.AppointmentId == appointmentId)
                {
                    MessageBox.Show("There is already an anamnesis in this appointemnt.");
                    return true;
                }
            }
            return false;
        }

        public Patient getPatient()
        {
            foreach (Patient patient in Singleton.Instance.patients)
            {
                if (patient.Id == anamnesis.PatientId)
                {
                    return patient;
                }
            }
            return null;
        }


        private void refreshMedicalRecord()
        {
            Patient patient = getPatient();
            MedicalRecord medicalRecord = patient.getMedicalRecord();
            medicalRecord.WriteAll(Singleton.Instance.medicalRecords);
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
            Singleton.Instance.anamnesis.Add(anamnesis);
            anamnesis.WriteAll(Singleton.Instance.anamnesis);
        }


    }
}
