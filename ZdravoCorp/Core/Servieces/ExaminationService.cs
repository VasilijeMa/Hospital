using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class ExaminationService
    {
        private IExaminationRepository examinationRepository;
        public ExaminationService()
        {
            this.examinationRepository = Singleton.Instance.ExaminationRepository;
        }

        public void WriteAll()
        {
            examinationRepository.WriteAll();
        }

        public void Add(Examination examination)
        {
            examinationRepository.Add(examination);
        }

        public Examination GetExaminationById(int examinationId)
        {
            return examinationRepository.GetExaminationById(examinationId);
        }
    }
}
