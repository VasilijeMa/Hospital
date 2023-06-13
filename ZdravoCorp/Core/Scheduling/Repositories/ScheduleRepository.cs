using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Core.Scheduling.Model;
using ZdravoCorp.Core.Scheduling.Repositories.Interfaces;
using ZdravoCorp.Core.UserManager.Model;

namespace ZdravoCorp.Core.Scheduling.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private Schedule schedule;

        public Schedule Schedule { get => schedule; }

        public ScheduleRepository()
        {
            schedule = new Schedule();
            schedule.Appointments = LoadAllAppointments();
            CreateAppointmentsMap();
            schedule.TodaysAppointments = GetTodaysAppontments();
        }

        public int getLastId()
        {
            try
            {
                return schedule.Appointments.Max(appointment => appointment.Id);
            }
            catch
            {
                return 0;
            }
        }

        public Appointment CreateAppointment(TimeSlot timeSlot, int doctorId, int patientId, string roomId = "", int idExamination = 0)
        {
            int id = getLastId() + 1;
            Appointment appointment = new Appointment(id, timeSlot, doctorId, patientId, roomId);
            schedule.Appointments.Add(appointment);
            if (appointment.TimeSlot.start.Date == DateTime.Now.Date) schedule.TodaysAppointments.Add(appointment);
            CreateAppointmentsMap();
            return appointment;
        }

        public Appointment CreateAppointment(Appointment appointment)
        {
            schedule.Appointments.Add(appointment);
            CreateAppointmentsMap();
            return appointment;
        }

        public Appointment UpdateAppointment(int appointmentId, TimeSlot timeSlot, int doctorId, Patient patient = null)
        {
            Appointment appointment = GetAppointmentById(appointmentId);
            appointment.TimeSlot = timeSlot;
            appointment.DoctorId = doctorId;
            if (patient != null)
            {
                appointment.PatientId = patient.Id;
            }
            return appointment;
        }

        public Appointment CancelAppointment(int appointmentId)
        {
            Appointment appointment = GetAppointmentById(appointmentId);
            appointment.IsCanceled = true;
            return appointment;
        }

        public List<Appointment> LoadAllAppointments()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/appointments.json");
            var json = reader.ReadToEnd();
            schedule.Appointments = JsonConvert.DeserializeObject<List<Appointment>>(json);
            return schedule.Appointments;
        }

        public void WriteAllAppointmens()
        {
            string json = JsonConvert.SerializeObject(schedule.Appointments, Formatting.Indented);
            File.WriteAllText("./../../../data/appointments.json", json);
        }

        public void CreateAppointmentsMap()
        {
            schedule.DailyAppointments = new Dictionary<DateTime, List<Appointment>>();
            foreach (var appointment in schedule.Appointments)
            {
                AddAppointmentToDailyMap(appointment.TimeSlot.start.Date, appointment);
                AddAppointmentToDailyMap(appointment.TimeSlot.start.AddMinutes(appointment.TimeSlot.duration).Date,
                    appointment);
            }
        }

        public void AddAppointmentToDailyMap(DateTime date, Appointment appointment)
        {
            if (!schedule.DailyAppointments.ContainsKey(date))
            {
                schedule.DailyAppointments.Add(date, new List<Appointment>());
            }
            schedule.DailyAppointments[date].Add(appointment);
        }

        public List<Appointment> GetTodaysAppontments()
        {
            List<Appointment> todayAppointments = new List<Appointment>();
            foreach (Appointment appointment in schedule.Appointments)
            {
                if (appointment.IsCanceled != false) continue;
                if (appointment.TimeSlot.start.ToShortDateString() != DateTime.Now.ToShortDateString()) continue;
                if (appointment.TimeSlot.start <= DateTime.Now) continue;
                todayAppointments.Add(appointment);
            }
            return todayAppointments;
        }

        public Appointment GetAppointmentById(int id)
        {
            return schedule.Appointments.FirstOrDefault(appointment => appointment.Id == id);
        }

        public List<Appointment> GetAppointmentsForPatient(int patientId)
        {
            return schedule.Appointments.Where(appointment => patientId == appointment.PatientId).ToList();
        }

        public List<Appointment> GetAppointmentsForDoctor(int doctorId)
        {
            return schedule.Appointments.Where(appointment => doctorId == appointment.DoctorId).ToList();
        }

        public List<Appointment> GetAppointmentsForPatientAndDoctor(int patientId, int doctorId)
        {
            return schedule.Appointments.Where(appointment => patientId == appointment.PatientId && doctorId == appointment.DoctorId).ToList();
        }

        public bool IsRoomScheduledForAppointment(string roomName, DateTime endDate)
        {
            bool isScheduled;
            foreach (var appointment in schedule.Appointments)
            {
                isScheduled = appointment.IdRoom == roomName && appointment.TimeSlot.start > endDate;
                if (isScheduled)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(DateTime date)
        {
            return schedule.DailyAppointments.ContainsKey(date);
        }

        public List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            return schedule.DailyAppointments[date];
        }

        public List<Appointment> GetTodaysAppointments()
        {
            return schedule.TodaysAppointments;
        }

        public List<Appointment> GetAppointments()
        {
            return schedule.Appointments;
        }

        public Appointment GetAppointmentByExaminationId(int examinationId)
        {
            foreach (var appointment in schedule.Appointments)
            {
                if (appointment.ExaminationId == examinationId) return appointment;
            }
            return null;
        }
    }
}
