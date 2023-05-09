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
        private string Where { get; set; }
        private string What { get; set; }
        private int Amount { get; set; }
        public FunctionalItem(string where, string what, int amount)
        {
            this.Where = where;
            this.What = what;
            this.Amount = amount;
        }
        public static List<FunctionalItem> LoadAll()
        {

            var serializer = new JsonSerializer();
            using StreamReader reader = new("./../../../data/functionalItems.json");
            var json = reader.ReadToEnd();
            List<FunctionalItem> allFunctionalItems = JsonConvert.DeserializeObject<List<FunctionalItem>>(json);

            return allFunctionalItems;
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

