using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.PhysicalAssets.Model
{
    public class MergeRenovation : Renovation
    {
        public string SecondRoomName;
        [JsonConstructor]
        public MergeRenovation(DateTime startDate, DateTime endDate, string roomName, int endType, string secondRoomName) : base(startDate, endDate, roomName, endType)
        {
            SecondRoomName = secondRoomName;
        }
        public MergeRenovation(Renovation renovation, string secondRoomName) : base(renovation.StartDate, renovation.EndDate, renovation.RoomName, (int)renovation.EndType)
        {
            SecondRoomName = secondRoomName;
        }
    }
}
