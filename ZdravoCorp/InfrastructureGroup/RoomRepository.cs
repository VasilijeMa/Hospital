using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.InfrastructureGroup
{
    public class RoomRepository
    {
        private Dictionary<string, Room> _rooms;

        public Dictionary<string, Room> LoadAll()
        {
            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/rooms.json");
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
        private void SaveAll()
        {
            SortedDictionary<string, Room> sortedRooms = new SortedDictionary<string, Room>(_rooms);
            string json = JsonConvert.SerializeObject(sortedRooms, Formatting.Indented);
            File.WriteAllText("./../../../data/rooms.json", json);
        }

        private void Add(Room room)
        {
            _rooms.Add(room.GetName(), room);
            SaveAll();
        }

        public bool ContainsRoom(string roomName)
        {
            return _rooms.ContainsKey(roomName);
        }

        private string GenerateName(RoomType roomType)
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
