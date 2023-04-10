using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp
{
    class Doctor
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public Specialization Specialization { get; set; }

        public Doctor() { }

        public Doctor(int id, string name, string lastname, Specialization specialization)
        {
            Id = id;
            Name = name;
            Lastname = lastname;
            Specialization = specialization;
        }

        public List<Appointment> GetAllAppointments(DateTime startDate, DateTime endDate)
        {
            List<Appointment> appointments =  new List<Appointment>();
            Schedule schedule = new Schedule();
            while (startDate.Day > endDate.Day) {
                startDate.AddDays(1);
                foreach (Appointment appointment in schedule.appointments)
                {
                    if (appointment.TimeSlot.start.Day == startDate.Day )
                    {
                        appointments.Add(appointment);
                    }
                }
            }
            return appointments;
        }

    }
}
