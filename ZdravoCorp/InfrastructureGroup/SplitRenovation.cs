using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.InfrastructureGroup
{
    public class SplitRenovation : Renovation
    {
        public RoomType SecondEndType { get; set; }
        [JsonConstructor]
        public SplitRenovation(DateTime startDate, DateTime endDate, string roomName, int endType, int secondEndType) : base(startDate, endDate, roomName, endType)
        {
            SecondEndType = (RoomType) secondEndType;
        }
        public SplitRenovation(Renovation renovation, int secondEndType) : base(renovation.StartDate, renovation.EndDate, renovation.RoomName, (int)(renovation.EndType))
        {
            SecondEndType = (RoomType)secondEndType;
        }
    }
}
