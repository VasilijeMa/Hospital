using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Services;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Services;

namespace ZdravoCorp
{

    public partial class DailyAppointmentView : Window
    {
        private List<Appointment> appointments;
        private Doctor doctor;
        private PatientService patientService = new PatientService();
        private ScheduleService scheduleService = new ScheduleService();
        private AnamnesisService anamnesisService;

        public DailyAppointmentView(List<Appointment> appointments, Doctor doctor, AnamnesisService anamnesisService)
        {
            InitializeComponent();
            this.doctor = doctor;
            this.appointments = appointments;
            LoadDataGrid();
            this.anamnesisService = anamnesisService;
        }

        private void LoadDataGrid()
        {
            DataTable dt = AddColumns();
            foreach (Appointment appointment in this.appointments)
            {
                dt.Rows.Add(
                    appointment.Id,
                    appointment.TimeSlot.start.Date.ToString("yyyy-MM-dd"),
                    appointment.TimeSlot.start.TimeOfDay.ToString(),
                    appointment.TimeSlot.duration,
                    appointment.DoctorId,
                    appointment.PatientId,
                    appointment.IsCanceled
                );
            }
            this.dataGrid.ItemsSource = dt.DefaultView;
        }

        private static DataTable AddColumns()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AppointmentID", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Duration", typeof(int));
            dt.Columns.Add("DoctorID", typeof(int));
            dt.Columns.Add("PatientID", typeof(int));
            dt.Columns.Add("IsCanceled", typeof(bool));
            return dt;
        }

        private void StartAppointmentClick(object sender, RoutedEventArgs e)
        {
            DataRowView item = dataGrid.SelectedItem as DataRowView;
            if (item == null)
            {
                MessageBox.Show("Appointment is not selected.");
                return;
            }

            int id = (int)item.Row["AppointmentId"];
            Appointment selectedAppointment = scheduleService.GetAppointmentById(id);
            Patient patient = patientService.GetById(selectedAppointment.PatientId);
            if (!selectedAppointment.IsAbleToStart())
            {
                MessageBox.Show("You cannot start a appointment.");
                return;
            }

            CreateMedicalRecordWindow medicalRecord = new CreateMedicalRecordWindow(false, patient, true, anamnesisService,selectedAppointment);
            medicalRecord.ShowDialog();
        }
    }
}

