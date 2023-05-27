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
        public bool IsFinished;
        public bool HasStarted;
        public DateTime StartDate;
        public DateTime EndDate;
        public string RoomName;
        public RoomType EndType;

        public Renovation(DateTime startDate, DateTime endDate, string roomName, int endType)
        {
            IsFinished = false;
            HasStarted = false;
            StartDate = startDate;
            EndDate = endDate;
            RoomName = roomName;
            EndType = (RoomType)endType;
        }
        public bool IsEligibleToStart()
        {
            return !HasStarted && (StartDate <= DateTime.Today);
        }

        public bool IsEligibleToFinish()
        {
            return !IsFinished && (EndDate <= DateTime.Today);
        }
    }
}
