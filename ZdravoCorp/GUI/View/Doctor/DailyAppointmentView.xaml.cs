using System.Collections.Generic;
using System.Data;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;

namespace ZdravoCorp
{

    public partial class DailyAppointmentView : Window
    {
        private List<Appointment> appointments;
        private Doctor doctor;
        Singleton singleton;

        public DailyAppointmentView(List<Appointment> appointments, Doctor doctor)
        {
            InitializeComponent();
            this.singleton = Singleton.Instance;
            this.doctor = doctor;
            this.appointments = appointments;
            LoadDataGrid();
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
            ScheduleRepository scheduleRepository = singleton.ScheduleRepository;
            Appointment selectedAppointment = scheduleRepository.GetAppointmentById(id);
            PatientRepository patientRepository = singleton.PatientRepository;
            Patient patient = patientRepository.getPatient(selectedAppointment.PatientId);
            if (!selectedAppointment.IsAbleToStart())
            {
                MessageBox.Show("You cannot start a appointment.");
                return;
            }

            CreateMedicalRecordWindow medicalRecord = new CreateMedicalRecordWindow(false, patient, true, selectedAppointment);
            medicalRecord.ShowDialog();
        }
    }
}

