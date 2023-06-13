using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.PhysicalAssets.Model;
using ZdravoCorp.Core.PhysicalAssets.Repositories.Interfaces;

namespace ZdravoCorp.Core.PhysicalAssets.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private Dictionary<string, Room> _rooms;

        public Dictionary<string, Room> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../../ZdravoCorp/data/rooms.json");
            var json = reader.ReadToEnd();
            Dictionary<string, Room> allRooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(json);
            return allRooms;
        }

        public RoomRepository()
        {
            _rooms = LoadAll();
        }

        public List<string> GetAllNames()
        {
            return _rooms.Keys.ToList();
        }
        public void SaveAll()
        {
            SortedDictionary<string, Room> sortedRooms = new SortedDictionary<string, Room>(_rooms);
            string json = JsonConvert.SerializeObject(sortedRooms, Formatting.Indented);
            File.WriteAllText("./../../../../ZdravoCorp/data/rooms.json", json);
        }

        public void Add(Room room)
        {
            _rooms.Add(room.GetName(), room);
            SaveAll();
        }

        public bool ContainsRoom(string roomName)
        {
            return _rooms.ContainsKey(roomName);
        }

        public string GenerateName(RoomType roomType)
        {
            string prefix = Room.GetTypeDescription(roomType) + " ";
            string newName = "";
            int i = 1;
            while (true)
            {
                newName = prefix + i.ToString();
                if (!ContainsRoom(newName))
                {
                    return newName;
                }
                i++;
            }
        }

        public Dictionary<string, Room> LoadAllExaminationRooms()
        {
            Dictionary<string, Room> examinationRooms = new Dictionary<string, Room>();
            foreach (var room in _rooms.Values)
            {
                if (RoomType.ExaminationRoom.Equals(room.GetTypeOfRoom()))
                {
                    examinationRooms.Add(room.GetName(), room);
                }
            }
            return examinationRooms;
        }

        public void MakeNewRoom(RoomType roomType)
        {
            string roomName = GenerateName(roomType);
            Room room = new Room((int)roomType, roomName);
            Add(room);
        }

        public void Delete(string roomName)
        {
            _rooms.Remove(roomName);
            SaveAll();
        }
    }
}
