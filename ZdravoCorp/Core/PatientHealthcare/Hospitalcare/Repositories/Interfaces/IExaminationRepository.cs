﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Repositories.Interfaces
{
    public interface IExaminationRepository
    {
        public List<Examination> LoadAll();

        public void WriteAll();

        public void Add(Examination examination);

        public Examination GetExaminationById(int examinationId);

        public List<Examination> GetExaminations();
    }
}
