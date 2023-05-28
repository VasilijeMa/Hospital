using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Repositories.Interfaces;

namespace ZdravoCorp.Core.Servieces
{
    public class AnamnesisService
    {
        private IAnamnesisRepository _anamnesisRepository;

        public AnamnesisService()
        {
            _anamnesisRepository = Singleton.Instance.AnamnesisRepository;
        }

        public List<Anamnesis> GetAnamneses()
        {
            return _anamnesisRepository.GetAnamneses();
        }

        public List<Anamnesis> GetAnamnesesContainingSubstring(string keyword)
        {
            return _anamnesisRepository.GetAnamnesesContainingSubstring(keyword);
        }

        public void WriteAll()
        {
            _anamnesisRepository.WriteAll();
        }

        public void AddAnamnesis(Anamnesis anamnesis)
        {
            _anamnesisRepository.AddAnamnesis(anamnesis);
        }
    }
}
