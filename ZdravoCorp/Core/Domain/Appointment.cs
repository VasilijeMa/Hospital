using System.Collections.Generic;
using System;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp.Core.Domain
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

    }
}
