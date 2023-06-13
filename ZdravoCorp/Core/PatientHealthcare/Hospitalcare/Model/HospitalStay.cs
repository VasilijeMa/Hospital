using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.PatientHealthcare.Hospitalcare.Model
{
    public class HospitalStay
    {
        public int ExaminationId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string RoomId { get; set; }

        public HospitalStay(int examinationId, DateOnly startDate, DateOnly endDate, string roomId)
        {
            ExaminationId = examinationId;
            StartDate = startDate;
            EndDate = endDate;
            RoomId = roomId;
        }
    }
}
