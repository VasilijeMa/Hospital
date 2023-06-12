using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PhysicalAssets.Model;

namespace ZdravoCorp.Core.PhysicalAssets.Repository.Interfaces
{
    public interface IWarehouseRepository
    {
        public Warehouse Load();
        public void Save();
        public void Save(Warehouse warehouse);
        public void AddItem(string key, int amount);
        public void AddItems(Dictionary<string, int> itemAmounts);
    }
}
