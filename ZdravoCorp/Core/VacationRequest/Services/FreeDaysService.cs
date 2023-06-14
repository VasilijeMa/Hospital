using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.CommunicationSystem.Repositories.Interfaces;
using ZdravoCorp.Core.VacationRequest.Model;
using ZdravoCorp.Core.VacationRequest.Repositories.Interfaces;

namespace ZdravoCorp.Core.VacationRequest.Services
{
    public class FreeDaysService
    {
        private IFreeDaysRepository _freeDaysRepository;

        public FreeDaysService(IFreeDaysRepository freeDaysRepository)
        {
            _freeDaysRepository = freeDaysRepository;
        }

        public void AddFreeDays(FreeDays freeDays)
        {
            _freeDaysRepository.AddFreeDays(freeDays);
        }
    }
}
