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

        public Medicament() { }

        public Medicament(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Medicament(int id, string name, List<string> allergens)
        {
            Id = id;
            Name = name;
            Allergens = allergens;
        }
    }
}
