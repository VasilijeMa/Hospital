using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.InfrastructureGroup
{
    public class MergeRenovation : Renovation
    {
        public string SecondRoomName;
        public MergeRenovation(DateTime startDate, DateTime endDate, string roomName, int endType, string secondRoomName) : base(startDate, endDate, roomName, endType)
        {
            SecondRoomName = secondRoomName;
        }
    }
}
