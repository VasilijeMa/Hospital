using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Documents;

public enum RoomType { OperatingTheatre, ExaminationRoom, Infirmary, WaitingRoom}
public class Room:Infrastructure
{
	private string Name;
	private RoomType Type;
	public Room(int type, string name, Dictionary<Equipment, int> inventory) : base(inventory)
	{
		this.Type = (RoomType)type;
		this.Name = name;
	}
}
