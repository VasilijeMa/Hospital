using System;
using ZdravoCorp.EquipmentGroup;

namespace ZdravoCorp.ManagerView
{
    public class EquipmentGridItem
    {
        public string Name { get; set; }
        public string TypeOfEq { get; set; }
        public int Quantity { get; set; }
        public EquipmentGridItem(string name, EquipmentType type)
        {
            Name = name;
            TypeOfEq = type.ToString();
            Quantity = 0;
        }

        public EquipmentGridItem(string name, string typeOfEq, int quantity)
        {
            Name = name;
            TypeOfEq = typeOfEq;
            Quantity = quantity;
        }

        public int GetQuantity() { return Quantity; }
        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public string GetName() { return Name; }
        public string GetTypeOfEq() { return TypeOfEq; }

        public string ToString()
        {
            return Name + " " + TypeOfEq.ToString() + " " + Quantity.ToString();
        }

    }
}