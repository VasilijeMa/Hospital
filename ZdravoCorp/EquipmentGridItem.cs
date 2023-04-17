using System;

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
		this.Name = name;
		this.TypeOfEq = typeOfEq;
		this.Quantity = quantity;
	}

	public int GetQuantity() { return  Quantity; }
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
