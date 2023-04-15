using System;

public class EquipmentGridItem
{
	public string Name { get; set; }
	public string Type { get; set; }
	public int Quantity { get; set; }
	public EquipmentGridItem(string name, EquipmentType type)
	{
		Name = name;
		Type = type.ToString();
		Quantity = 0;
	}

	
}
