using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IMedicamentToAddRepository
    {
        public List<MedicamentToAdd> LoadAll();

        public void WriteAll();

        public MedicamentToAdd GetMedicamenToAddtById(int id);

        public List<MedicamentToAdd> GetMedicamentsToAdd();
    }
}
