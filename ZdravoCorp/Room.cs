using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Windows.Documents;

public enum RoomType {
	OperatingTheatre = 1,
	ExaminationRoom = 2,
	Infirmary = 3,
	WaitingRoom = 4
}
public class Room:Infrastructure
{
	private RoomType Type { get; set; }
    public Room(int type, string name) : base(name)
	{
		this.Type = (RoomType)type;
	}

    public static Dictionary<string, Room> LoadAll()
    {

        var serializer = new JsonSerializer();
        using StreamReader reader = new("./../../../data/rooms.json");
        var json = reader.ReadToEnd();
        Dictionary<string, Room> allRooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(json);
        return allRooms;
    }

    public string ToString()
    {
        return Name + " " + Type.ToString();
    }
}
