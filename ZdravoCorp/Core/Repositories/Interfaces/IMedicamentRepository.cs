﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IMedicamentRepository
    {
        public List<Medicament> LoadAll();

        public void WriteAll();

        public Medicament GetMedicamentById(int id);

        public Medicament GetByName(string name);

        public List<Medicament> GetMedicaments();
    }
}
