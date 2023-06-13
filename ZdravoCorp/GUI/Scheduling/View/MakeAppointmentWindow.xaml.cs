using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MakeAppointmentWindow.xaml
    /// </summary>
    public partial class MakeAppointmentWindow : Window
    {
        const int APPOINTMENT_DURATION = 15;
        Patient patient;
        List<Doctor> doctors;
        private DoctorService doctorService = new DoctorService();
        private ScheduleService scheduleService = new ScheduleService();
        private PatientService patientService = new PatientService();
        private LogService logService = new LogService();
        public MakeAppointmentWindow(Patient patient, Doctor doctor = null)
        {
            InitializeComponent();
            this.patient = patient;
            LoadData(doctor);
        }

        private void LoadData(Doctor doctor)
        {
            doctors = doctorService.GetDoctors();
            cmbDoctors.ItemsSource = doctors;
            cmbDoctors.ItemTemplate = (DataTemplate)FindResource("doctorTemplate");
            cmbDoctors.SelectedValuePath = "Id";
            SetDoctor(doctor);
            dpDate.DisplayDateStart = DateTime.Now;
        }

        private void SetDoctor(Doctor doctor)
        {
            if (doctor != null)
                cmbDoctors.SelectedItem = doctors.FirstOrDefault(
                    selectedDoctor =>
                        selectedDoctor.FirstName == doctor.FirstName && selectedDoctor.LastName == doctor.LastName);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!inputValidation())
            {
                return;
            }
            if (IsTimeSlotValid(out var timeSlot)) return;
            Doctor doctor = (Doctor)cmbDoctors.SelectedItem;
            if (IsTimeSlotAvailable(timeSlot, doctor)) return;
            Appointment appointment = scheduleService.CreateAppointment(timeSlot, doctor, patient);
            logService.AddElement(appointment, patient);
            MessageBox.Show("Appointment successfully created.");
            this.Close();
        }

        private bool IsTimeSlotAvailable(TimeSlot timeSlot, Doctor doctor)
        {
            if (!doctorService.IsAvailable(timeSlot, doctor.Id))
            {
                MessageBox.Show("Doctor is not available at choosen date and time.");
                return true;
            }

            if (!patientService.IsAvailable(timeSlot, patient.Id))
            {
                MessageBox.Show("Patient is not available at choosen date and time.");
                return true;
            }

            return false;
        }

        private bool IsTimeSlotValid(out TimeSlot timeSlot)
        {
            timeSlot = MakeTimeSlot();
            if (timeSlot.start <= DateTime.Now)
            {
                MessageBox.Show("The selected date cannot be in the past.");
                return true;
            }

            return false;
        }

        public TimeSlot MakeTimeSlot()
        {
            int hour = int.Parse(tbTime.Text.Split(":")[0]);
            int minutes = int.Parse(tbTime.Text.Split(":")[1]);
            DateTime dtValue = dpDate.SelectedDate.Value;
            DateTime dateTime = new DateTime(dtValue.Year, dtValue.Month, dtValue.Day, hour, minutes, 0);
            return new TimeSlot(dateTime, APPOINTMENT_DURATION);
        }

        public bool inputValidation()
        {
            if (tbTime.Text == "" || dpDate.SelectedDate == null || cmbDoctors.SelectedItem == null)
            {
                MessageBox.Show("Fill in all the fields");
                return false;
            }
            string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbTime.Text, pattern))
            {
                MessageBox.Show("Please enter a valid time value in \"HH:mm\" format.");
                return false;
            }
            return true;
        }
    }
}
