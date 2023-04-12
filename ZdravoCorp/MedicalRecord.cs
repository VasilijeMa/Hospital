using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ZdravoCorp
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public List<int> AnamnesesId { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public MedicalRecord() { }

        public MedicalRecord(int id, List<int> anamnesesId, double height, double weight)
        {
            Id = id;
            AnamnesesId = anamnesesId;
            Height = height;
            Weight = weight;
        }
    }
}
