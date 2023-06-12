using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PhysicalAssets.Model;

namespace ZdravoCorp.Core.PhysicalAssets.Repository.Interfaces
{
    public interface IRenovationRepository<T> where T : Renovation
    {
        public List<T> LoadAll();
        public void SaveAll();

        public void Add(T renovation);

        public bool IsRoomScheduledForRenovation(string roomName);

        public List<T> ExtractNotYetExecutedRenovations(bool onlyNotYetStarted);
    }
}
