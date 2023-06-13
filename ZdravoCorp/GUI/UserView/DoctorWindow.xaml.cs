using System;
using System.Collections.Generic;
using System.Windows;
using ZdravoCorp.Core.CommunicationSystem.Services;
using ZdravoCorp.Core.PatientHealthcare.PatientMedicalRecord.Services;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Services;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.VacationRequest.Services;
using ZdravoCorp.GUI.View.Doctor;
using ZdravoCorp.GUI.View.Patient;

namespace ZdravoCorp
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        private Doctor doctor { get; set; }
        private ScheduleService scheduleService = new ScheduleService();
        private ChatService chatService;
        private FreeDaysService freeDaysService;
        private NotificationAboutCancelledAppointmentService notifications =
            new NotificationAboutCancelledAppointmentService();
        private AnamnesisService anamnesisService;


        public DoctorWindow(Doctor doctor, ChatService chatService, FreeDaysService freeDaysService, AnamnesisService anamnesisService)
        {
            InitializeComponent();
            this.doctor = doctor;
            this.chatService = chatService;
            this.freeDaysService = freeDaysService;
            SetFields(doctor);
            showNotification(doctor.Id);
            this.anamnesisService = anamnesisService;
        }
        public void showNotification(int doctorId)
        {
            foreach (NotificationAboutCancelledAppointment notification in notifications.GetNotifications())
            {
                if ((notification.DoctorId == doctorId) && (!notification.isShown))
                {
                    MessageBox.Show("Your appointment with id: " + notification.AppointmenntId.ToString() + " is cancalled.");
                    notification.isShown = true;
                    notifications.AddNotification(notification);
                    notifications.WriteAll(notifications.GetNotifications());
                    return;
                }
            }
        }
        private void SetFields(Doctor doctor)
        {
            nameTxt.Text = doctor.FirstName;
            lastNameTxt.Text = doctor.LastName;
            idTxt.Text = doctor.Id.ToString();
            specializationTxt.Text = doctor.Specialization.ToString();
        }
        private void MakeAppointmentClick(object sender, RoutedEventArgs e)
        {
            MakeAppointmentDoctor appointmentDoctor = new MakeAppointmentDoctor(doctor, false);
            appointmentDoctor.Show();
        }

        private void ViewOneDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            var appointments = scheduleService.GetAllAppointmentsForDoctor(DateTime.Now.Date, DateTime.Now.Date, doctor.Id);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 1);
            appointmentDoctor.Show();
        }

        private void ViewThreeDayAppointmentClick(object sender, RoutedEventArgs e)
        {
            DateTime endDate = DateTime.Now.AddDays(3);
            var appointments = scheduleService.GetAllAppointmentsForDoctor(DateTime.Now.Date, endDate.Date, doctor.Id);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 3);
            appointmentDoctor.Show();
        }

        private void DailyScheduleClick(object sender, RoutedEventArgs e)
        {
            var appointments = scheduleService.GetAllAppointmentsForDoctor(DateTime.Now.Date, DateTime.Now.Date, doctor.Id);
            DailyAppointmentView dailySchedule = new DailyAppointmentView(appointments, doctor, anamnesisService);
            dailySchedule.ShowDialog();
        }

        private void SearchPatientClick(object sender, RoutedEventArgs e)
        {
            SearchPatientWindow searchPatient = new SearchPatientWindow(doctor, anamnesisService);
            searchPatient.Show();
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            scheduleService.WriteAllAppointmens();
        }

        private void FreeDays_Click(object sender, RoutedEventArgs e)
        {
            FreeDaysView freeDaysView = new FreeDaysView(doctor, freeDaysService);
            freeDaysView.ShowDialog();
        }

        private void Visit_Click(object sender, RoutedEventArgs e)
        {
            HospitalizedPatientView hospitalizedPatientView = new HospitalizedPatientView(doctor);
            hospitalizedPatientView.ShowDialog();
        }
        
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ChatsView chatsView = new ChatsView(doctor, chatService);
            chatsView.ShowDialog();
        }
    }
}
