using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ZdravoCorp.EquipmentGroup
{
    public enum EquipmentType
    {
        Examination = 1,
        Surgery = 2,
        Furniture = 3,
        Hallway = 4
    };
    public class Equipment
    {
        string Name { get; set; }
        EquipmentType TypeOfEq { get; set; }

        bool Dynamic { get; set; }
        public Equipment(int typeOfEq, string name, bool Dynamic)
        {
            this.TypeOfEq = (EquipmentType)typeOfEq;
            this.Name = name;
            this.Dynamic = Dynamic;
        }

        public EquipmentType GetTypeOfEq() { return this.TypeOfEq; }
        public string GetName() { return this.Name; }

        public string ToString()
        {
            return Name + ", " + TypeOfEq.ToString() + ", Dynamic: " + Dynamic.ToString();
        }

        public bool IsDynamic()
        {
            return Dynamic;
        }
    }


}

