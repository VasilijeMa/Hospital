using System;
using System.Collections.Generic;

namespace ZdravoCorp.InfrastructureGroup
{
    public class Infrastructure
    {

        protected string Name { get; set; }
        public Infrastructure(string name)
        {
            this.Name = name;
        }
    }
}
