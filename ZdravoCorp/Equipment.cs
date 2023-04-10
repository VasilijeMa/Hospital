using System;
public enum EquipmentType { Examination, Surgery, Furniture, Hallway };
public class Equipment
{
	string Name;
	EquipmentType Type;
	public Equipment(int type, string name)
	{
		this.Type = (EquipmentType)type;
		this.Name = name;
	}
	public string GetName()
	{
		return this.Name;
	}
}
