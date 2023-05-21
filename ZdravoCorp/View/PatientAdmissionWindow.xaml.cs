﻿using System.Data;
using System.Windows;
using System.Windows.Forms;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;
using ZdravoCorp.Servieces;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for PatientAdmissionWindow.xaml
    /// </summary>
    public partial class PatientAdmissionWindow : Window
    {
        public PatientAdmissionWindow()
        {
            InitializeComponent();
            LoadData();
        }

        public DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Appointment ID");
            dt.Columns.Add("Timeslot");
            dt.Columns.Add("Doctor's username");
            dt.Columns.Add("Doctor's first name");
            dt.Columns.Add("Doctor's last name");
            dt.Columns.Add("Patient's username");
            dt.Columns.Add("Patient's first name");
            dt.Columns.Add("Patient's last name");
            dt.AcceptChanges();
            return dt;
        }
        public void LoadData()
        {
            DataTable dt = CreateDataTable();
            foreach (Appointment appointment in Singleton.Instance.Schedule.TodaysAppointments)
            {
                DoctorRepository doctorRepository = new DoctorRepository();
                PatientRepository patientRepository = new PatientRepository();
                Doctor doctor = doctorRepository.getDoctor(appointment.DoctorId);
                Patient patient = patientRepository.getPatient(appointment.PatientId);
                dt.Rows.Add(appointment.Id.ToString()
                    , appointment.TimeSlot.start.ToString()
                    , doctor.Username
                    , doctor.FirstName
                    , doctor.LastName
                    , patient.Username
                    , patient.FirstName
                    , patient.LastName);
                dt.AcceptChanges();
            }
            datagrid.DataContext = dt.DefaultView;
        }

        private void AddAnamnesis_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = datagrid.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("You must select the appointment first to add an anamnesis.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return;
            }

            Appointment selectedAppointment = Singleton.Instance.Schedule.TodaysAppointments[selectedIndex];
            AppointmentService appointmentService = new AppointmentService();
            if (!appointmentService.IsAbleToStart(selectedAppointment))
            {
                MessageBox.Show("You cannot start a appointment.");
                return;
            }
            //isAlreadyExsist(selectedAppointment.Id);
            PatientRepository patientRepository = new PatientRepository();
            Patient patient = patientRepository.getPatient(selectedAppointment.PatientId);
            CreateMedicalRecordWindow medicalRecordWindow = new CreateMedicalRecordWindow(false, patient, false, selectedAppointment);
            //AnamnesisView anamnesisView = new AnamnesisView(selectedAppointment,true);
            medicalRecordWindow.ShowDialog();
        }
    }
}
