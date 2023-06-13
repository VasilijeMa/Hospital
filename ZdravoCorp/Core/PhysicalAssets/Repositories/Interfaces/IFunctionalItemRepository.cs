using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PhysicalAssets.Services;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces
{
    public interface IFunctionalItemRepository
    {
        public List<FunctionalItem> LoadAll();

        public FunctionalItem FindOrMakeCombination(string roomName, string equipmentName);

        public void SaveAll();
        public void SaveAll(List<FunctionalItem> allRequests);

        public Dictionary<string, int> EmptyOutRoom(string roomName);
    }
}
