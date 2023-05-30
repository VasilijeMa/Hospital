using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class Medicament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Allergens { get; set; }

        public int Quantity { get; set; }
        public Medicament(int id, string name, List<string> allergens, int quantity)
        {
            Id = id;
            Name = name;
            Allergens = allergens;
            Quantity = quantity;    
        }
    }
}
