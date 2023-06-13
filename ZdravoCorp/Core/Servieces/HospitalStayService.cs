using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;

namespace ZdravoCorp.Core.Servieces
{
    public class HospitalStayService
    {
        private HospitalStayRepository hospitalStayRepository;

        public HospitalStayService() 
        {
            hospitalStayRepository = new HospitalStayRepository();
        }

        public void Add(HospitalStay hospitalStay)
        {
            hospitalStayRepository.Add(hospitalStay);
        }

        public HospitalStay GetHospitalStay(int examinationId)
        {
            return hospitalStayRepository.GetHospitalStay(examinationId);
        }

        public List<HospitalStay> LoadAll()
        {
            return hospitalStayRepository.LoadAll();
        }

        public void WriteAll()
        {
            hospitalStayRepository.WriteAll();
        }

        public List<string> FindFreeRooms(DateOnly startDate, DateOnly endDate) 
        {
            return null;
            //idi kroz sobe 
            //gledaj infirmary rooms
            //istovremeno prolazi kroz hospital stay i gledaj id sobe
            //ukoliko je soba slobodna u tom periodu dodaj u listu
            //vrati tu listu i napuni kombobox
        }
    }
}
