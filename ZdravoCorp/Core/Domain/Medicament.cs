using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.Domain
{
    public class Medicament
    {
        public string Name;
        public List<string> allergens;

        public Medicament(string name, List<string> allergens)
        {
            Name = name;
            this.allergens = allergens;
        }


    }
}
