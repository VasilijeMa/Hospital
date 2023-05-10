using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.InfrastructureGroup;

namespace ZdravoCorp
{
    public class Appointment
    {
        public int Id { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public bool IsCanceled { get; set; }
        public string IdRoom { get; set; }

        public Appointment(int id, TimeSlot timeSlot, int doctorId, int patientId, string idRoom)
        {
            Id = id;
            TimeSlot = timeSlot;
            DoctorId = doctorId;
            PatientId = patientId;
            IsCanceled = false;
            IdRoom = idRoom;
        }

        public Appointment() { }

        /*public void startAppointment()
        {
            Patient patient = Singleton.Instance.patients[PatientId];
            //CreateMedicalRecordWindow medicalRecord = CreateMedicalRecordWindow();
        }*/

        public Doctor getDoctor() {
            foreach (Doctor doctor in Singleton.Instance.doctors)
            {
                if (doctor.Id == this.DoctorId)
                {
                    return doctor;
                }
            }
            return null;
        }
        public Patient getPatient()
        {
            foreach (Patient patient in Singleton.Instance.patients)
            {
                if (patient.Id == this.PatientId)
                {
                    return patient;
                }
            }
            return null;
        }

        public bool isAbleToStart()
        {
            DateTime earliestStart = TimeSlot.start.Add(new TimeSpan(0, -15, 0));
            DateTime latestStart = TimeSlot.start.Add(new TimeSpan(0, TimeSlot.duration, 0));

            if (DateTime.Now < earliestStart || DateTime.Now > latestStart)
            {
                return false;
            }
            return true;
        }

        public static String takeRoom(TimeSlot timeSlot)
        {
            Dictionary<String, Room> examinationRooms = Room.LoadAllExaminationRoom();
            foreach (var room in examinationRooms)
            {
                bool check = true;
                foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
                {
                    if (appointment.IsCanceled) continue;
                    if (appointment.TimeSlot.OverlapWith(timeSlot) && appointment.IdRoom == room.Key)
                    {
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    return room.Key;
                }
            }
            return "";
        }
    }
}
