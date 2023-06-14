using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.VacationRequest.Model;

namespace ZdravoCorp.Core.VacationRequest.Repositories.Interfaces
{
    public interface IFreeDaysRepository
    {
        public List<FreeDays> LoadAll();

        public void WriteAll();

        public void AddFreeDays(FreeDays freeDay);

        public List<FreeDays> GetAll();

        public void SaveAll(List<FreeDays> remainingRequests);
    }
}
