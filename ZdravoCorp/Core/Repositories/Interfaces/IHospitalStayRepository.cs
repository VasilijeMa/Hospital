﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IHospitalStayRepository
    {
        HospitalStay GetHospitalStay(int examinationId);

        public void WriteAll();

        public void Add(HospitalStay hospitalStay);

        public List<HospitalStay> LoadAll();

    }
}
