using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.VacationRequest.Model
{
    public class ProcessedVacationRequest : FreeDays
    {
        public bool IsApproved { get; set; }
        public ProcessedVacationRequest(int id, DateTime startDate, int duration, string reason, bool isApproved) : base(id, startDate, duration, reason)
        {
            IsApproved = isApproved;
        }
    }
}
