namespace ZdravoCorp.Core.PhysicalAssets.Model
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
            TypeOfEq = (EquipmentType)typeOfEq;
            Name = name;
            this.Dynamic = Dynamic;
        }

        public EquipmentType GetTypeOfEq() { return TypeOfEq; }
        public string GetName() { return Name; }

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

