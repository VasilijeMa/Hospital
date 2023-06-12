using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IFreeDaysRepository
    {
        public List<FreeDays> LoadAll();

        public void WriteAll();

        public void AddFreeDays(FreeDays freeDay);
    }
}
