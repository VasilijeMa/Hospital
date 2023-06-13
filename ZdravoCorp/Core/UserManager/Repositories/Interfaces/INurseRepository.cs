using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.UserManager.Model;

namespace ZdravoCorp.Core.UserManager.Repositories.Interfaces
{
    public interface INurseRepository
    {
        public List<Nurse> LoadAll();

        public List<Nurse> GetNurses();
    }
}
