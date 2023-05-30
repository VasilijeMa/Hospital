using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface INurseRepository
    {
        public List<Nurse> LoadAll();

        public List<Nurse> GetNurses();
    }
}
