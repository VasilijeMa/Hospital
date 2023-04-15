using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;

public enum EquipmentType {
	Examination = 1,
    Surgery = 2, 
	Furniture = 3, 
	Hallway = 4
};
public class Equipment
{
	string Name { get; set; }
	EquipmentType Type { get; set; }
    public Equipment(int type, string name)
	{
		this.Type = (EquipmentType)type;
		this.Name = name;
	}

	public static Dictionary<string, Equipment> LoadAll()
	{

        var serializer = new JsonSerializer();
        using StreamReader reader = new("./../../../data/equipment.json");
        var json = reader.ReadToEnd();
        Dictionary<string, Equipment> allEquipment = JsonConvert.DeserializeObject<Dictionary<string, Equipment>>(json);
        return allEquipment;
    }

	public string ToString()
	{
		return Name + " " + Type.ToString();
	}
}
