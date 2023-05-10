using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ZdravoCorp
{
    public class Doctor:User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Specialization Specialization { get; set; }

        public Doctor():base() { }

        public Doctor(int id, string name, string lastname, Specialization specialization, string username, string password, string type) : base(username, password, type)
        {
            Id = id;
            FirstName = name;
            LastName = lastname;
            Specialization = specialization;
        }

        public List<Appointment> GetAllAppointments(DateTime startDate, DateTime endDate)
        {
            List<Appointment> appointments =  new List<Appointment>();

            while (startDate <= endDate)
            {
                foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
                {
                    if (appointment.TimeSlot.start == startDate && appointment.DoctorId == this.Id)
                    {
                        appointments.Add(appointment);
                    }
                }
                startDate = startDate.AddDays(1);
            }
            return appointments;
        }
        public static List<Doctor> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/doctor.json");
            var json = reader.ReadToEnd();
            List<Doctor> doctors = JsonConvert.DeserializeObject<List<Doctor>>(json);
            return doctors;
        }

        public Doctor GetDoctor(int id)
        {
            foreach(Doctor doctor in Singleton.Instance.doctors)
            {
                if (doctor.Id == id) return doctor;
            }
            return null;
        }

        public bool IsAvailable(TimeSlot timeSlot, int appointmentId = -1)
        {
            foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
            {
                if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
                if (Id == appointment.DoctorId && appointment.TimeSlot.OverlapWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
