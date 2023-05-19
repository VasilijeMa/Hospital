using System;
using System.Collections.Generic;
using System.Data;
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
using ZdravoCorp.Domain;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    public partial class EmergencyOperationOrExamination : Window
    {
        private int typedDuration;
        private String selectedSpecialization;
        private String selectedPatient;
        private List<Appointment> appointmentsToCancel;
        public EmergencyOperationOrExamination()
        {
            InitializeComponent();
            delayButton.Visibility= Visibility.Hidden;
            fillCheckBoxes();
        }

        private void fillCheckBoxes()
        {
            addSpecializationsToComboBox();
            addPatientToComboBox();
            addSearchCriteria();
        }

        private void addSpecializationsToComboBox() {
            foreach (Doctor doctor in Singleton.Instance.doctors){
                specialization.Items.Add(doctor.Specialization);
            }
        }

        private void addPatientToComboBox() {
            foreach (Patient patient in Singleton.Instance.patients) {
                patients.Items.Add(patient.Username);
            }
        }

        private void addSearchCriteria() {
            examinationOrOperation.Items.Add("Examination");
            examinationOrOperation.Items.Add("Operation");
        }

        private bool isOperationSelected() {
            if (examinationOrOperation.SelectedItem.ToString() == "Operation") {
                return true;
            }
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (areComboboxesSelected())
            {
                this.selectedSpecialization = specialization.SelectedItem.ToString();
                this.selectedPatient = patients.SelectedItem.ToString();
                Doctor doctor = new Doctor();
                List<Doctor> qualifiedDoctors = doctor.getDoctorBySpecialization(selectedSpecialization);
                MessageBox.Show(qualifiedDoctors.Count().ToString());
                Doctor firstFoundDoctor = getFirstFreeDoctor(qualifiedDoctors);
                if (firstFoundDoctor == null)
                {
                    this.appointmentsToCancel = getAppointmentsInNextTwoHours(qualifiedDoctors);
                    if (this.appointmentsToCancel.Count == 0)
                    {
                        ShowInTable();
                    }
                    else
                    {
                        MessageBox.Show("There is no appointments in next two hours.");
                        this.Close();

                    }
                }
                else
                {
                    MessageBox.Show("Doctor for this emergency appointment is " + firstFoundDoctor.FirstName + " " + firstFoundDoctor.LastName + ".");
                    this.Close();
                }
            }
        }

      private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Appointment ID");
            dt.Columns.Add("Start date");
            dt.Columns.Add("Duration");
            dt.Columns.Add("Doctor's username");
            dt.Columns.Add("Patient's username");
            dt.Columns.Add("Room ID");
            dt.AcceptChanges();
            return dt;
        }

        private void ShowInTable() {
            DataTable dt = CreateTable();
            delayButton.Visibility = Visibility.Visible;
            foreach (Appointment appointment in this.appointmentsToCancel)
            {
                dt.Rows.Add(appointment.Id, 
                    appointment.TimeSlot.start.ToString(),
                    appointment.TimeSlot.duration.ToString(),
                    appointment.getDoctor().Username,
                    appointment.getPatient().Username,
                    appointment.IdRoom.ToString());
                dt.AcceptChanges();
            }
            datagrid.DataContext = dt.DefaultView;
        }
        private int getDuration() {
            if (isOperationSelected()) {
                this.typedDuration = int.Parse(operationDuration.Text);
                return this.typedDuration;
            }
            return 15;
        }

        private List<Appointment> getAppointmentsInNextTwoHours(List<Doctor> qualifiedDoctors)
        {
            List<Appointment> allAppointments = new List<Appointment>();
            foreach (Doctor doctor in qualifiedDoctors) {
                List <Appointment> appointmentsForOne = doctor.GetAppointmentsInNextTwoHours();
                if (appointmentsForOne.Count() == 0) {continue;}
                foreach (Appointment appointment in appointmentsForOne) {
                    allAppointments.Add(appointment);
                }
            }
            return allAppointments;
        }
        private Doctor getFirstFreeDoctor(List<Doctor> qualifiedDoctors) {
            DateTime currentTime = DateTime.Now;
            DateTime timeAfterTwoHours = DateTime.Now.AddHours(2);
            foreach(Doctor qualifiedDoctor in qualifiedDoctors)
            {
                for (DateTime time = currentTime;time < timeAfterTwoHours;time=time.AddMinutes(5)) {
                    TimeSlot doctorsTimeSlot = new TimeSlot(currentTime,getDuration());
                    if (qualifiedDoctor.IsAvailable(doctorsTimeSlot)){
                            Appointment appointment = Singleton.Instance.Schedule.CreateAppointment(doctorsTimeSlot,
                            qualifiedDoctor, Patient.getByUsername(this.selectedPatient));
                            if (appointment != null){
                                Singleton.Instance.Schedule.WriteAllAppointmens();
                            }
                            return qualifiedDoctor;
                    }
                
                }

            }
            return null;
        }

        private bool areComboboxesSelected() {
            if ((specialization.SelectedItem == null) ||
                (patients.SelectedItem == null) ||
                (examinationOrOperation.SelectedItem == null)) {
                MessageBox.Show("You can't leave comboboxes empty.");
                return false;
            }
            if (isOperationSelected())
            {
                if (operationDuration.Text == "")
                {
                    MessageBox.Show("You can't field empty.");
                    return false;
                }
            }
            else {
                if (operationDuration.Text != "")
                {
                    MessageBox.Show("You should leave field empty.");
                    return false;
                }
            }
            return true;
        
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int selectedIndex = datagrid.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("You must select the patient whose account you want to edit.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
            }
            else {
                Appointment selectedAppointment = this.appointmentsToCancel[selectedIndex];
                selectedAppointment.IsCanceled = true;
                Appointment emergencyAppointment = Singleton.Instance.Schedule.CreateAppointment(selectedAppointment.TimeSlot,
                            selectedAppointment.getDoctor(), Patient.getByUsername(this.selectedPatient));
                NotificationAboutCancelledAppointment notification = new NotificationAboutCancelledAppointment
                    (emergencyAppointment.Id,emergencyAppointment.DoctorId,false);
                Singleton.Instance.notificationAboutCancelledAppointment.Add(notification);

                MessageBox.Show(Singleton.Instance.notificationAboutCancelledAppointment.Count().ToString());
                notification.WriteAll(Singleton.Instance.notificationAboutCancelledAppointment);
                if (emergencyAppointment != null)
                {
                    Singleton.Instance.Schedule.WriteAllAppointmens();
                    MessageBox.Show("Emergency appointment added successfully.");
                    this.Close();
                }

            }
        }
    }
}
