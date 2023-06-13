using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.UserManager.Model;

namespace ZdravoCorp.Core.UserManager.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        public List<Patient> LoadAll();

        public void WriteAll();

        public Patient getPatient(int patientId);

        public Patient getById(int id);

        public Patient getByUsername(string username);

        public int GetMedicalRecordId(int patientId);

        public List<Patient> GetPatients();

        public void AddPatient(Patient patient);

        public void RemovePatient(Patient patient);
    }
}
