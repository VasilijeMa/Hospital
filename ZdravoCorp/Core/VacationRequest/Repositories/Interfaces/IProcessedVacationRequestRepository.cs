using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.VacationRequest.Model;

namespace ZdravoCorp.Core.VacationRequest.Repositories.Interfaces
{
    public interface IProcessedVacationRequestRepository
    {
        public List<ProcessedVacationRequest> LoadAll();
        public void SaveAll();
        public void Add(ProcessedVacationRequest request);
    }
}
