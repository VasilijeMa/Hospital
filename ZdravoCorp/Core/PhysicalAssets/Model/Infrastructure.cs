using Newtonsoft.Json;

namespace ZdravoCorp.Core.PhysicalAssets.Model
{
    public class Infrastructure
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        public Infrastructure(string name)
        {
            Name = name;
        }

        public string GetName() { return Name; }
    }
}
