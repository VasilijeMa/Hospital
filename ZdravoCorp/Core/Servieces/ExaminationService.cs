using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;

namespace ZdravoCorp.Core.Servieces
{
    public class ExaminationService
    {
        private ExaminationRepository examinationRepository;
        private ScheduleRepository scheduleRepository;
        public ExaminationService()
        {
            this.scheduleRepository = Singleton.Instance.ScheduleRepository;
            this.examinationRepository = Singleton.Instance.ExaminationRepository;
        }

        public void AddSpecializationReferral()
        {

        }
        public void AddHospitalizationReferral()
        {

        }

        public void AddPrescription()
        {

        }
    }
}
