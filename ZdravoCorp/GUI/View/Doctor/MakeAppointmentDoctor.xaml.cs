using System;
using System.Windows;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Servieces;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MakeAppointmentDoctor.xaml
    /// </summary>
    public partial class MakeAppointmentDoctor : Window
    {
        Singleton singleton;
        Doctor doctor;
        bool update;
        int appointmentId;
        public MakeAppointmentDoctor(Doctor doctor, bool update = false, int appointentId = -1)
        {
            this.doctor = doctor;
            this.appointmentId = appointentId;
            singleton = Singleton.Instance;
            this.update = update;
            InitializeComponent();
            cmbPatients.ItemsSource = singleton.PatientRepository.Patients;
            cmbPatients.ItemTemplate = (DataTemplate)FindResource("patientTemplate");
            cmbPatients.SelectedValuePath = "Id";
            dpDate.DisplayDateStart = DateTime.Now;
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
            TimeSlot timeSlot = MakeTimeSlot();
            DoctorService doctorService = new DoctorService();
            PatientService patientService = new PatientService();
            if (!doctorService.IsAvailable(timeSlot, doctor.Id, appointmentId))
            {
                MessageBox.Show("You are not available at choosen date and time.");
                return;
            }
            Patient patient = (Patient)cmbPatients.SelectedItem;
            if (!patientService.IsAvailable(timeSlot, patient.Id, appointmentId))
            {
                MessageBox.Show("Patient is not available at choosen date and time.");
                return;
            }
            ScheduleRepository scheduleRepository = singleton.ScheduleRepository;
            if (update)
            {
                scheduleRepository.UpdateAppointment(appointmentId, timeSlot, doctor.Id, (Patient)cmbPatients.SelectedItem);
                MessageBox.Show("Appointment successfully updated.");
                this.Close();
                return;
            }
            else
            {
                scheduleRepository.CreateAppointment(timeSlot, doctor, (Patient)cmbPatients.SelectedItem);
                MessageBox.Show("Appointment successfully created.");
                this.Close();
            }
        }

        public TimeSlot MakeTimeSlot()
        {
            int hour = int.Parse(tbTime.Text.Split(":")[0]);
            int minutes = int.Parse(tbTime.Text.Split(":")[1]);
            DateTime dtValue = dpDate.SelectedDate.Value;
            DateTime dateTime = new DateTime(dtValue.Year, dtValue.Month, dtValue.Day, hour, minutes, 0);
            return new TimeSlot(dateTime, int.Parse(tbDuration.Text));
        }

        public bool inputValidation()
        {
            if (tbDuration.Text == "" || tbTime.Text == "" || dpDate.SelectedDate == null || cmbPatients.SelectedItem == null)
            {
                MessageBox.Show("Fill in all the fields");
                return false;
            }
            string pattern = @"^([01][0-9]|2[0-3]):[0-5][0-9]$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(tbTime.Text, pattern))
            {
                MessageBox.Show("Please enter a valid time value in \"hh:mm\" format.");
                return false;
            }
            return true;
        }
    }
}
