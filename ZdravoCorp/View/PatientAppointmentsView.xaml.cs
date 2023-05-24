using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Controllers;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.Enums;
using ZdravoCorp.Repositories;
using ZdravoCorp.Servieces;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for PatientAppointmentsView.xaml
    /// </summary>
    public partial class PatientAppointmentsView : Window
    {
        List<Anamnesis> anamneses;
        Singleton singleton;
        Patient patient;
        public PatientAppointmentsView(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            singleton = Singleton.Instance;
            anamneses = singleton.AnamnesisRepository.Anamneses;
            LoadAppointmentsInDataGrid();
            LoadMedicalRecordInToxtBox();
        }

        private void LoadMedicalRecordInToxtBox()
        {
            MedicalRecordController medicalRecordController = new MedicalRecordController();
            MedicalRecord medicalRecord = medicalRecordController.GetMedicalRecord(patient.MedicalRecordId);
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
                    ScheduleRepository scheduleRepository = singleton.ScheduleRepository;
                    Appointment appointment = scheduleRepository.GetAppointmentById(anamnesis.AppointmentId);
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
            ScheduleRepository scheduleRepository = singleton.ScheduleRepository;
            Appointment appointment = scheduleRepository.GetAppointmentById((int)item.Row["Id"]);
            AnamnesisView anamnesisView = new AnamnesisView(appointment, ConfigRoles.Patient);
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
                anamneses = singleton.AnamnesisRepository.GetAnamnesesContainingSubstring(tbSearch.Text.ToUpper());
            }
            LoadAppointmentsInDataGrid();
            anamneses = singleton.AnamnesisRepository.Anamneses;
        }
    }
}
