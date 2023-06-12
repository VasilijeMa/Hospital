﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Domain;

namespace ZdravoCorp.Core.Repositories.Interfaces
{
    public interface IDoctorSurveyRepository
    {
        public List<DoctorSurvey> LoadAll();

        public void WriteAll();

        public DoctorSurvey GetById(int id);

        public void AddSurvey(int appointmentId, string username, int doctorId, int serviceQuality, int suggestToFriends, string comment);

        public void UpdateSurvey(int appointmentId, int serviceQuality, int suggestToFriends, string comment);
    }
}
