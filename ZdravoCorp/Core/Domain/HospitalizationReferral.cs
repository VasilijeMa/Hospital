﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class HospitalizationReferral
    {
        public int Duration { get; set; }

        public string AdditionalTesting { get; set; }

        public List<Medicament> InitialTherapy { get; set; }

        public HospitalizationReferral() { }

        public HospitalizationReferral(int duration, List<Medicament> initialTherapy, string additionalTesting)
        {
            Duration = duration;
            InitialTherapy = initialTherapy;
            AdditionalTesting = additionalTesting;
        }
    }
}