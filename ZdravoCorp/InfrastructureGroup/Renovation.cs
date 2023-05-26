using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.InfrastructureGroup
{
    public class Renovation
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public string RoomName;
        public RoomType EndType;

        [JsonConstructor]
        public Renovation(DateTime startDate, DateTime endDate, string roomName, int endType) {
            StartDate = startDate;
            EndDate = endDate;
            RoomName = roomName;
            EndType = (RoomType)endType;
        }
    }
}
