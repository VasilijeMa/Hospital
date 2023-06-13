using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Repositories;
using ZdravoCorp.Core.UserManager.Services;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    public partial class EmergencyOperationOrExamination : Window
    {
        private int typedDuration;
        private String selectedSpecialization;
        private String selectedPatient;
        private List<Appointment> appointmentsToCancel;
        private DoctorService doctorService = new DoctorService();
        private PatientService patientService = new PatientService();
        private ScheduleService scheduleService = new ScheduleService();
        private NotificationAboutCancelledAppointmentService notificationService = new NotificationAboutCancelledAppointmentService();
        public EmergencyOperationOrExamination()
        {
            InitializeComponent();
            delayButton.Visibility = Visibility.Hidden;
            fillCheckBoxes();
        }
        private void fillCheckBoxes()
        {
            addSpecializationsToComboBox();
            addPatientToComboBox();
            addSearchCriteria();
        }
        private void addSpecializationsToComboBox()
        {
            foreach (Doctor doctor in doctorService.GetDoctors())
            {
                specialization.Items.Add(doctor.Specialization);
            }
        }
        private void addPatientToComboBox()
        {
            foreach (Patient patient in patientService.GetPatients())
            {
                patients.Items.Add(patient.Username);
            }
        }
        private void addSearchCriteria()
        {
            examinationOrOperation.Items.Add("Examination");
            examinationOrOperation.Items.Add("Operation");
        }
        private bool isOperationSelected()
        {
            if (examinationOrOperation.SelectedItem.ToString() == "Operation")
            {
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
                List<Doctor> qualifiedDoctors = doctorService.GetDoctorBySpecialization(selectedSpecialization);
                MessageBox.Show(qualifiedDoctors.Count().ToString());
                Doctor firstFoundDoctor = doctorService.getFirstFreeDoctor(qualifiedDoctors, getDuration(), selectedPatient);
                if (firstFoundDoctor == null)
                {
                    this.appointmentsToCancel = doctorService.getAppointmentsInNextTwoHours(qualifiedDoctors);
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
        private void ShowInTable()
        {
            DataTable dt = CreateTable();
            delayButton.Visibility = Visibility.Visible;
            foreach (Appointment appointment in this.appointmentsToCancel)
            {
                DoctorRepository doctorRepository = new DoctorRepository();
                PatientRepository patientRepository = new PatientRepository();
                dt.Rows.Add(appointment.Id,
                    appointment.TimeSlot.start.ToString(),
                    appointment.TimeSlot.duration.ToString(),
                    doctorRepository.getDoctor(appointment.DoctorId).Username,
                    patientRepository.getPatient(appointment.PatientId).Username,
                    appointment.IdRoom.ToString());
                dt.AcceptChanges();
            }
            datagrid.DataContext = dt.DefaultView;
        }
        private int getDuration()
        {
            if (isOperationSelected())
            {
                this.typedDuration = int.Parse(operationDuration.Text);
                return this.typedDuration;
            }
            return 15;
        }
        private bool areComboboxesSelected()
        {
            if ((specialization.SelectedItem == null) ||
                (patients.SelectedItem == null) ||
                (examinationOrOperation.SelectedItem == null))
            {
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
            else
            {
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
            else
            {
                Appointment selectedAppointment = this.appointmentsToCancel[selectedIndex];
                selectedAppointment.IsCanceled = true;
                Appointment emergencyAppointment = scheduleService.CreateAppointment(selectedAppointment.TimeSlot,
                    doctorService.GetDoctor(selectedAppointment.DoctorId), patientService.GetByUsername(this.selectedPatient));
                NotificationAboutCancelledAppointment notification = new NotificationAboutCancelledAppointment
                    (emergencyAppointment.Id, emergencyAppointment.DoctorId, false);
                notificationService.AddNotification(notification);

                MessageBox.Show(notificationService.GetNotifications().Count().ToString());
                notificationService.WriteAll(notificationService.GetNotifications());
                if (emergencyAppointment != null)
                {
                    scheduleService.WriteAllAppointmens();
                    MessageBox.Show("Emergency appointment added successfully.");
                    this.Close();
                }

            }
        }
    }
}
