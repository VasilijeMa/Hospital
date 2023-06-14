using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Core.Enums;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Model;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Services;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Repositories;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for PatientAppointmentsView.xaml
    /// </summary>
    public partial class PatientAppointmentsView : Window
    {
        List<Anamnesis> anamneses;
        //Institution singleton;
        Patient patient;
        AnamnesisService anamnesisService;
        private ScheduleService scheduleService = new ScheduleService();

        public PatientAppointmentsView(Patient patient, AnamnesisService anamnesisService)
        {
            InitializeComponent();
            this.anamnesisService = anamnesisService;
            this.patient = patient;
            anamneses = anamnesisService.GetAnamneses();
            LoadAppointmentsInDataGrid();
            LoadMedicalRecordInToxtBox();
        }

        private void LoadMedicalRecordInToxtBox()
        {
            MedicalRecordService medicalRecordService = new MedicalRecordService();
            MedicalRecord medicalRecord = medicalRecordService.GetMedicalRecordById(patient.MedicalRecordId);
            tbAllergens.Text = ListToString(medicalRecord.Allergens);
            tbEarlierIllnesses.Text = ListToString(medicalRecord.EarlierIllnesses);
            tbHeight.Text = medicalRecord.Height.ToString();
            tbWeight.Text = medicalRecord.Weight.ToString();
        }

        private string ListToString(List<string> strings)
        {
            return string.Join(",", strings);
        }

        private void LoadAppointmentsInDataGrid()
        {
            dgAppointments.ItemsSource = null;
            DataTable dt = AddColumns();
            foreach (Anamnesis anamnesis in anamneses)
            {
                if (anamnesis.PatientId == patient.Id)
                {
                    Appointment appointment = scheduleService.GetAppointmentById(anamnesis.AppointmentId);
                    DoctorRepository doctorRepository = new DoctorRepository();
                    Doctor doctor = doctorRepository.getDoctor(appointment.DoctorId);
                    if (appointment.TimeSlot.start.Date > DateTime.Now.Date) continue;
                    dt.Rows.Add(appointment.Id, appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"),
                        appointment.TimeSlot.start.TimeOfDay.ToString(@"hh\:mm"), doctor.FirstName + " " + doctor.LastName,
                        doctor.Specialization.ToString(), anamnesis.DoctorsObservation + "\n" + anamnesis.DoctorsConclusion);
                }
            }
            dgAppointments.ItemsSource = dt.DefaultView;
        }

        private static DataTable AddColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("DoctorName", typeof(string));
            dt.Columns.Add("DoctorSpetialization", typeof(string));
            dt.Columns.Add("Anamnesis", typeof(string));
            return dt;
        }

        private void btnOpenAnamnesis_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = dgAppointments.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }
            Appointment appointment = scheduleService.GetAppointmentById((int)item.Row["Id"]);
            AnamnesisView anamnesisView = new AnamnesisView(appointment, anamnesisService, ConfigRoles.Patient);
            anamnesisView.ShowDialog();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAppointments.SelectedItem == null)
            {
                return;
            }
            btnOpenAnamnesis.IsEnabled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text != "")
            {
                anamneses = anamnesisService.GetAnamnesesContainingSubstring(tbSearch.Text.ToUpper());
            }
            LoadAppointmentsInDataGrid();
            anamneses = anamnesisService.GetAnamneses();
        }
    }
}
