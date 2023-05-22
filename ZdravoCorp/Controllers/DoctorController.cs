using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;
using ZdravoCorp.Servieces;

namespace ZdravoCorp.Controllers
{
    public class DoctorController
    {
        private DoctorService doctorService = new DoctorService();

        public void ViewOneDayAppointment(Doctor doctor)
        {
            DoctorService doctorService = new DoctorService();
            var appointments = doctorService.GetAllAppointments(DateTime.Now.Date, DateTime.Now.Date, doctor.Id);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 1);
            appointmentDoctor.Show();
        }

        public void ViewThreeDayAppointment(Doctor doctor)
        {
            DateTime endDate = DateTime.Now.AddDays(3);
            var appointments = doctorService.GetAllAppointments(DateTime.Now.Date, endDate.Date, doctor.Id);
            ViewAppointment appointmentDoctor = new ViewAppointment(appointments, doctor, 3);
            appointmentDoctor.Show();
        }

        public void DailySchedule(Doctor doctor)
        {
            var appointments = doctorService.GetAllAppointments(DateTime.Now.Date, DateTime.Now.Date, doctor.Id);
            DailyAppointmentView dailySchedule = new DailyAppointmentView(appointments, doctor);
            dailySchedule.ShowDialog();
        }

        public void SearchPatient(Doctor doctor)
        {
            SearchPatientWindow searchPatient = new SearchPatientWindow(doctor);
            searchPatient.Show();
        }

        public void MakeAppointment(Doctor doctor)
        {
            MakeAppointmentDoctor appointmentDoctor = new MakeAppointmentDoctor(doctor, false);
            appointmentDoctor.Show();
        }

        public void showNotification(int doctorId)
        {
            NotificationAboutCancelledAppointmentRepository notificationAppointmentRepository =
                new NotificationAboutCancelledAppointmentRepository();

            foreach (NotificationAboutCancelledAppointment notification in Singleton.Instance.NotificationAboutCancelledAppointmentRepository.Notifications)
            {
                if ((notification.DoctorId == doctorId) && (!notification.isShown))
                {
                    MessageBox.Show("Your appointment with id: " + notification.AppointmenntId.ToString() + " is cancalled.");
                    notification.isShown = true;
                    notificationAppointmentRepository.Add(notification);
                    NotificationAboutCancelledAppointmentRepository.WriteAll(notificationAppointmentRepository.Notifications);
                    return;
                }
            }
        }

        public List<Doctor> GetDoctorBySpecialization(string specialization)
        {
            return doctorService.GetDoctorBySpecialization(specialization);
        }

        public List<Appointment> getAppointmentsInNextTwoHours(List<Doctor> qualifiedDoctors)
        {
            return doctorService.getAppointmentsInNextTwoHours(qualifiedDoctors);
        }

        public Doctor GetFirstFreeDoctor(List<Doctor> qualifiedDoctors, int duration, string patientUsername)
        {
            return doctorService.getFirstFreeDoctor(qualifiedDoctors, duration, patientUsername);
        }
    }
}
