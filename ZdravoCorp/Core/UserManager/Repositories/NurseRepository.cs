﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ZdravoCorp.Core.UserManager.Model;
using ZdravoCorp.Core.UserManager.Repositories.Interfaces;

namespace ZdravoCorp.Core.UserManager.Repositories
{
    public class NurseRepository : INurseRepository
    {
        private List<Nurse> nurses;
        public List<Nurse> Nurses { get => nurses; }
        public NurseRepository()
        {
            nurses = LoadAll();
        }
        public List<Nurse> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/nurse.json");
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<Nurse>>(json);
        }

        public List<Nurse> GetNurses()
        {
            return nurses;
        }
    }
}
