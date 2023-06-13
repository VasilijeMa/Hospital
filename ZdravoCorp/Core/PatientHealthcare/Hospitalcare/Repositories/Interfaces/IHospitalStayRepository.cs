using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Repositories.Interfaces
{
    public interface IHospitalStayRepository
    {
        HospitalStay GetHospitalStay(int examinationId);

        public void WriteAll();

        public void Add(HospitalStay hospitalStay);

        public List<HospitalStay> LoadAll();

        public int GetNumberOfPatientsInTheRoom(string roomId, DateOnly startDate, DateOnly endDate);
    }
}
