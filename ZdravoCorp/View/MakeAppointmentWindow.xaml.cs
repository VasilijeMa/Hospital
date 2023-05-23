using System;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;
using ZdravoCorp.Servieces;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for MakeAppointmentWindow.xaml
    /// </summary>
    public partial class MakeAppointmentWindow : Window
    {
        const int APPOINTMENT_DURATION = 15;
        Singleton singleton;
        Patient patient;
        public MakeAppointmentWindow(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            singleton = Singleton.Instance;
            cmbDoctors.ItemsSource = singleton.DoctorRepository.Doctors;
            cmbDoctors.ItemTemplate = (DataTemplate)FindResource("doctorTemplate");
            cmbDoctors.SelectedValuePath = "Id";
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
            if (timeSlot.start <= DateTime.Now)
            {
                MessageBox.Show("The selected date cannot be in the past.");
                return;
            }
            Doctor doctor = (Doctor)cmbDoctors.SelectedItem;
            DoctorService doctorService = new DoctorService();
            PatientService patientService = new PatientService();
            if (!doctorService.IsAvailable(timeSlot, doctor.Id))
            {
                MessageBox.Show("Doctor is not available at choosen date and time.");
                return;
            }
            if (!patientService.IsAvailable(timeSlot, patient.Id))
            {
                MessageBox.Show("Patient is not available at choosen date and time.");
                return;
            }

            ScheduleRepository scheduleRepository = singleton.ScheduleRepository;
            Appointment appointment = scheduleRepository.CreateAppointment(timeSlot, doctor, patient);

            LogService logService = new LogService();
            logService.AddElement(appointment, patient);
            MessageBox.Show("Appointment successfully created.");
            this.Close();
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
