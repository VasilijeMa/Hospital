using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Documents;

public enum RoomType { OperatingTheatre, ExaminationRoom, Infirmary, WaitingRoom}
public class Room
{
	private string Name;
	private RoomType Type;
	private Dictionary<Equipment, int> Inventory;
	public Room(int type, string name, Dictionary<Equipment, int> inventory)
	{
		this.Type = (RoomType)type;
		this.Name = name;
		this.Inventory = inventory;
	}
	void Add(Equipment equipment)
	{
		if (this.Inventory.ContainsKey(equipment))
		{
			this.Inventory[equipment]++;
		}
		else
		{
			this.Inventory.Add(equipment, 1);
		}
	}
}
