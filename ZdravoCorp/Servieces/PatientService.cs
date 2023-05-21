using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Servieces
{
    public class PatientService
    {
        //Patient patient;

        //public PatientService(Patient patient)
        //{
        //    this.patient = patient;
        //}

        public static Patient getById(int id)
        {
            foreach (Patient patient in PatientRepository.Patients)
            {
                if (patient.Id == id)
                {
                    return patient;
                }
            }
            return null;
        }

        public static Patient getByUsername(string username)
        {
            foreach (Patient patient in PatientRepository.Patients)
            {
                if (patient.Username == username)
                {
                    return patient;
                }
            }
            return null;
        }

    }
}
