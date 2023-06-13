using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;
using ZdravoCorp.Core.Scheduling.Model;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IAnamnesisRepository
    {
        public List<Anamnesis> LoadAll();

        public void WriteAll();

        public List<Anamnesis> GetAnamnesesContainingSubstring(string keyword);

        public Anamnesis findAnamnesisById(Appointment selectedAppointment);

        public List<Anamnesis> GetAnamneses();

        public void AddAnamnesis(Anamnesis anamnesis);
    }
}
