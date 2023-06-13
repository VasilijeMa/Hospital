namespace ZdravoCorp.Core.PhysicalAssets.Model
{
    public class EquipmentQuantity
    {
        public string Name { get; set; }
        public string TypeOfEq { get; set; }
        public int Quantity { get; set; }
        public EquipmentQuantity(string name, EquipmentType type)
        {
            Name = name;
            TypeOfEq = type.ToString();
            Quantity = 0;
        }

        public EquipmentQuantity(string name, string typeOfEq, int quantity)
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