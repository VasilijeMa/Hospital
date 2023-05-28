using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class MedicamentService
    {
        private IMedicamentRepository _medicamentRepository;

        public MedicamentService()
        {
            _medicamentRepository = Singleton.Instance.MedicamentRepository;
        }

        public List<Medicament> GetMedicaments()
        {
            return _medicamentRepository.GetMedicaments();
        }
    }
}
