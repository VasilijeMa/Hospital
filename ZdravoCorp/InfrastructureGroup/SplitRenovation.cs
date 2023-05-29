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
        public SplitRenovation(DateTime startDate, DateTime endDate, string roomName, int endType, int secondEndType) : base(startDate, endDate, roomName, endType)
        {
            SecondEndType = (RoomType) secondEndType;
        }
    }
}
