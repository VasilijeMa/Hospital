using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.EquipmentGroup
{
    public class AlteredEquipmentQuantity
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("maxQuantity")]
        public int MaxQuantity { get; set; }
        [JsonProperty("selectedQuantity")]
        public int SelectedQuantity { get; set; }
        public AlteredEquipmentQuantity(string name, int maxQuantity)
        {
            Name = name;
            MaxQuantity = maxQuantity;
            SelectedQuantity=0;
        }

        [JsonConstructor]
        public AlteredEquipmentQuantity(string name, int maxQuantity, int selectedQuantity)
        {
            Name = name;
            MaxQuantity = maxQuantity;
            SelectedQuantity = selectedQuantity;
        }

        public int GetSelectedQuantity() { return SelectedQuantity; }

        public string GetName() { return Name; }
        public int GetMaxQuantity() { return MaxQuantity; }

        public string ToString()
        {
            return Name + " " + MaxQuantity.ToString() + " " + SelectedQuantity.ToString();
        }
    }
}
