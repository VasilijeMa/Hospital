﻿using System;
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
	EquipmentType TypeOfEq { get; set; }
    public Equipment(int typeOfEq, string name)
	{
		this.TypeOfEq = (EquipmentType)typeOfEq;
		this.Name = name;
	}

	public static List<Equipment> LoadAll()
	{

        var serializer = new JsonSerializer();
        using StreamReader reader = new("./../../../data/equipment.json");
        var json = reader.ReadToEnd();
        List<Equipment> allEquipment = JsonConvert.DeserializeObject<List<Equipment>>(json);
        return allEquipment;
    }

	public EquipmentType GetTypeOfEq() { return this.TypeOfEq; }
	public string GetName() { return this.Name; }

	public string ToString()
	{
		return Name + " " + TypeOfEq.ToString();
	}
}
