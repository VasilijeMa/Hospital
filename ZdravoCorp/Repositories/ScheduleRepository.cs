using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZdravoCorp.Domain;
using ZdravoCorp.Servieces;

namespace ZdravoCorp.Repositories
{
    public class ScheduleRepository
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

        public Appointment CreateAppointment(TimeSlot timeSlot, Doctor doctor, Patient patient)
        {
            string roomId = Appointment.TakeRoom(timeSlot);
            if (roomId == "")
            {
                MessageBox.Show("All rooms are full.");
                return null;
            }
            int id = getLastId() + 1;
            Appointment appointment = new Appointment(id, timeSlot, doctor.Id, patient.Id, roomId);
            schedule.Appointments.Add(appointment);
            if (appointment.TimeSlot.start.Date == DateTime.Now.Date) schedule.TodaysAppointments.Add(appointment);
            CreateAppointmentsMap();
            return appointment;
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
        public Appointment CreateAppointment(Appointment appointment)
        {
            schedule.Appointments.Add(appointment);
            CreateAppointmentsMap();
            return appointment;
        }

        public Appointment UpdateAppointment(int appointmentId, TimeSlot timeSlot, int doctorId, Patient patient = null)
        {
            foreach (var appointment in schedule.Appointments)
            {
                if (appointment.Id == appointmentId)
                {
                    appointment.TimeSlot = timeSlot;
                    appointment.DoctorId = doctorId;
                    if (patient != null)
                    {
                        appointment.PatientId = patient.Id;
                    }
                    return appointment;
                }
            }
            return null;
        }

        public Appointment CancelAppointment(int appointmentId)
        {
            foreach (var appointment in schedule.Appointments)
            {
                if (appointment.Id == appointmentId)
                {
                    appointment.IsCanceled = true;
                    return appointment;
                }
            }
            return null;
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
                if (schedule.DailyAppointments.ContainsKey(appointment.TimeSlot.start.Date))
                {
                    schedule.DailyAppointments[appointment.TimeSlot.start.Date].Add(appointment);
                }
                else
                {
                    schedule.DailyAppointments.Add(appointment.TimeSlot.start.Date, new List<Appointment>());
                    schedule.DailyAppointments[appointment.TimeSlot.start.Date].Add(appointment);
                }
                if (schedule.DailyAppointments.ContainsKey(appointment.TimeSlot.start.AddMinutes(appointment.TimeSlot.duration).Date))
                {
                    schedule.DailyAppointments[appointment.TimeSlot.start.AddMinutes(appointment.TimeSlot.duration).Date].Add(appointment);
                }
                else
                {
                    schedule.DailyAppointments.Add(appointment.TimeSlot.start.AddMinutes(appointment.TimeSlot.duration).Date, new List<Appointment>());
                    schedule.DailyAppointments[appointment.TimeSlot.start.AddMinutes(appointment.TimeSlot.duration).Date].Add(appointment);
                }
            }
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
            foreach (var appointment in schedule.Appointments)
            {
                if (appointment.Id == id)
                {
                    return appointment;
                }
            }
            return null;
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
    }
}
