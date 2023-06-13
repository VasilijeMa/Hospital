using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PhysicalAssets.Model;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        public Dictionary<string, Room> LoadAll();

        public List<string> GetAllNames();
        public void SaveAll();

        public void Add(Room room);

        public bool ContainsRoom(string roomName);

        public string GenerateName(RoomType roomType);

        public Dictionary<string, Room> LoadAllExaminationRooms();

        public void MakeNewRoom(RoomType roomType);

        public void Delete(string roomName);
    }
}
