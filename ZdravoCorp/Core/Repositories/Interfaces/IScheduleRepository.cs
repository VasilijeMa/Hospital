using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        public Appointment CreateAppointment(TimeSlot timeSlot, int doctorId, int patientId, string roomId = "", int idExamination = 0);

        public Appointment CreateAppointment(Appointment appointment);

        public Appointment UpdateAppointment(int appointmentId, TimeSlot timeSlot, int doctorId,
            Patient patient = null);

        public Appointment CancelAppointment(int appointmentId);

        public List<Appointment> LoadAllAppointments();

        public void WriteAllAppointmens();

        public void CreateAppointmentsMap();

        public void AddAppointmentToDailyMap(DateTime date, Appointment appointment);

        public List<Appointment> GetTodaysAppontments();

        public Appointment GetAppointmentById(int id);

        public List<Appointment> GetAppointmentsForPatient(int patientId);

        public List<Appointment> GetAppointmentsForDoctor(int doctorId);

        public List<Appointment> GetAppointmentsForPatientAndDoctor(int patientId, int doctorId);

        public bool ContainsKey(DateTime date);

        public int getLastId();

        public List<Appointment> GetAppointmentsByDate(DateTime date);

        public List<Appointment> GetTodaysAppointments();

        public List<Appointment> GetAppointments();

        public Appointment GetAppointmentByExaminationId(int examinationId);

        public bool IsRoomScheduledForAppointment(string roomName, DateTime endDate);
    }
}
