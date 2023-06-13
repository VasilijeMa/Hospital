using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Repositories
{
    public class HospitalStayRepository : IHospitalStayRepository
    {
        public List<HospitalStay> HospitalStays;

        public HospitalStayRepository() {
            HospitalStays = LoadAll();
        }
       

        public void Add(HospitalStay hospitalStay)
        {
            HospitalStays.Add(hospitalStay);
            WriteAll();
        }

        public HospitalStay GetHospitalStay(int examinationId)
        {
            foreach (HospitalStay hospitalStay in HospitalStays) 
            {
                if (hospitalStay.ExaminationId == examinationId) 
                {
                    return hospitalStay;
                }
            }
            return null;
        }

        public List<HospitalStay> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/hospitalstay.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<HospitalStay>>(json);
        }

        public void WriteAll()
        {
            string json = JsonConvert.SerializeObject(HospitalStays, Formatting.Indented);
            File.WriteAllText("./../../../data/hospitalstay.json", json);
        }

        public int GetNumberOfPatientsInTheRoom(string roomId,DateOnly startDate, DateOnly endDate)
        {
            int numberOfPatients = 0;
            foreach (HospitalStay hospitalStay in LoadAll())
            {
                if (hospitalStay.RoomId == roomId)
                {
                    if ((startDate >= hospitalStay.StartDate && startDate <= hospitalStay.EndDate) || (endDate >= hospitalStay.StartDate && endDate <=hospitalStay.EndDate))
                    {
                        numberOfPatients += 1;
                    }
                }
            }
            return numberOfPatients;
        }
    }
}
