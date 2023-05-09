using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
    }
}
