using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Domain
{
    public class Medicament
    {
        public String Name;
        public List<String> allergens;

        public Medicament(string name, List<string> allergens)
        {
            Name = name;
            this.allergens = allergens;
        }


    }
}
