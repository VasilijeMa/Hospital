using Newtonsoft.Json;

namespace ZdravoCorp.InfrastructureGroup
{
    public class Infrastructure
    {
        [JsonProperty("name")]
        protected string Name { get; set; }
        public Infrastructure(string name)
        {
            this.Name = name;
        }

        public string GetName() { return this.Name; }
    }
}
