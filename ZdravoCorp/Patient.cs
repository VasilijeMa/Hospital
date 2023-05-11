using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ZdravoCorp
{
    public class Patient:User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public int MedicalRecordId { get; set; }
        public bool IsBlocked { get; set; }
        public Patient(int id, string firstName, string lastName, DateOnly birthDate, int medicalRecordId, string username, string password, string type, bool isBlocked) :base(username, password, type)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.BirthDate = birthDate;
            this.MedicalRecordId = medicalRecordId;
            this.IsBlocked = isBlocked;
        }
        public Patient():base() { }

        public static List<Patient> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/patient.json");
            var json = reader.ReadToEnd();
            List<Patient> patients = JsonConvert.DeserializeObject<List<Patient>>(json);
            return patients;
        }

        public void WriteAll(List<Patient> newlistofpatients)
        {
            string json = JsonConvert.SerializeObject(newlistofpatients, Formatting.Indented);
            File.WriteAllText("./../../../data/patient.json", json);
        }

        public bool IsAvailable(TimeSlot timeSlot, int appointmentId = -1)
        {
            foreach (Appointment appointment in Singleton.Instance.Schedule.appointments)
            {
                if (appointment.Id == appointmentId || appointment.IsCanceled) continue;
                if (Id == appointment.PatientId && appointment.TimeSlot.OverlapWith(timeSlot))
                {
                    return false;
                }
            }
            return true;
        }

        public static Patient getById(int id)
        {
            foreach (Patient patient in Singleton.Instance.patients)
            {
                if (patient.Id == id)
                {
                    return patient;
                }
            }
            return null;
        }

        public static Patient getByUsername(String username) {
            foreach (Patient patient in Singleton.Instance.patients) {
                if (patient.Username == username) {
                    return patient;
                }
            }
            return null;
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
