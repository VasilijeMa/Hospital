using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Documents;

public enum RoomType { OperatingTheatre, ExaminationRoom, Infirmary, WaitingRoom}
public class Room:Infrastructure
{
	private RoomType Type;
	public Room(int type, string name) : base(name)
	{
		this.Type = (RoomType)type;
	}
}
