using System.Collections.Generic;
using System;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Scheduling.Model
{
    public class Appointment
    {
        public int Id { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public bool IsCanceled { get; set; }
        public string IdRoom { get; set; }
        public int ExaminationId { get; set; }

        public Appointment(int id, TimeSlot timeSlot, int doctorId, int patientId, string idRoom)
        {
            Id = id;
            TimeSlot = timeSlot;
            DoctorId = doctorId;
            PatientId = patientId;
            IsCanceled = false;
            IdRoom = idRoom;
            //ExaminationId = Id;
        }

        public Appointment() { }

        public bool IsAbleToStart()
        {
            DateTime earliestStart = TimeSlot.start.Add(new TimeSpan(0, -15, 0));
            DateTime latestStart = TimeSlot.start.Add(new TimeSpan(0, TimeSlot.duration, 0));

            if (DateTime.Now < earliestStart || DateTime.Now > latestStart)
            {
                return false;
            }
            return true;
        }

        public bool IsCancellable(int doctorId, TimeSlot timeSlot)
        {
            return ((DoctorId == doctorId) && (!IsCanceled) && (TimeSlot.OverlapWith(timeSlot)));
        }

        public string InfoForPatient()
        {
            return "ID: " + Id.ToString() + " Start Time: " + TimeSlot.start.ToString() + " Duration: " + TimeSlot.duration.ToString() + "mins\n" +
                "Doctor ID: " + DoctorId + " Room: " + IdRoom;
        }
    }
}
