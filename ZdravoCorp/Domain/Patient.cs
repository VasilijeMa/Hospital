using System;
using ZdravoCorp.Servieces;

namespace ZdravoCorp.Domain
{
    public class Patient : User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public int MedicalRecordId { get; set; }
        public bool IsBlocked { get; set; }
        public Patient(int id, string firstName, string lastName, DateOnly birthDate, int medicalRecordId, string username, string password, string type, bool isBlocked) : base(username, password, type)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            MedicalRecordId = medicalRecordId;
            IsBlocked = isBlocked;
        }
        public Patient() : base() { }
        public bool IsAvailable(TimeSlot timeSlot, int appointmentId = -1)
        {
            foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
            {
                if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
                TimeSlotService timeSlotService = new TimeSlotService(appointment.TimeSlot);
                if (Id == appointment.PatientId && timeSlotService.OverlapWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }
        public MedicalRecord getMedicalRecord()
        {
            foreach (MedicalRecord medicalRecord in Singleton.Instance.medicalRecords)
            {
                if (MedicalRecordId == medicalRecord.Id)
                {
                    return medicalRecord;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return "Id: " + Id + ", FirstName: " + FirstName + ", LastName: " + LastName + "BirthDate: " + BirthDate.ToString();
        }

    }
}
