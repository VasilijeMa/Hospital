using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using ZdravoCorp.EquipmentGroup;

namespace ZdravoCorp.InfrastructureGroup
{
    public class FunctionalItem
    {
        public string Where { get; set; }
        public string What { get; set; }
        public int Amount { get; set; }
        public FunctionalItem(string where, string what, int amount)
        {
            this.Where = where;
            this.What = what;
            this.Amount = amount;
        }

        public void SetAmount(int amount)
        {
            this.Amount = amount;
        }

        public string GetWhere() { return Where; }
        public string GetWhat() { return What; }
        public int GetAmount() { return Amount; }
        public string ToString()
        {
            return What + " " + Where + " " + Amount.ToString();
        }
    }
}

