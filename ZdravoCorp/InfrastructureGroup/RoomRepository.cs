using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ZdravoCorp.InfrastructureGroup
{
    internal class RoomRepository
    {
        public static Dictionary<string, Room> LoadAll()
        {

            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/rooms.json");
            var json = reader.ReadToEnd();
            Dictionary<string, Room> allRooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(json);
            return allRooms;
        }
    }
}
