using System;
public enum EquipmentType { Examination, Surgery, Furniture, Hallway };
public class Equipment
{
	
	EquipmentType Type;
	bool IsInWarehouse;
	public Equipment(bool isInWarehouse, int type)
	{
		this.Type = (EquipmentType)type;
		this.IsInWarehouse = isInWarehouse;
	}
}
