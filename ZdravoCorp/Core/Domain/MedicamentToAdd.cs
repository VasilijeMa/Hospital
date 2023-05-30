using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class MedicamentToAdd
    {
        public int MedicamentId { get; set; }
        public int NewQuantity { get; set; }
        public DateOnly DateOfOrder { get; set; }
        public bool IsDone { get; set; }
        public MedicamentToAdd(int medicamentId, int newQuantity, DateOnly date, bool isDone) 
        {
            MedicamentId = medicamentId;
            NewQuantity = newQuantity;
            DateOfOrder = date;
            IsDone = isDone;
        }
    }
}
