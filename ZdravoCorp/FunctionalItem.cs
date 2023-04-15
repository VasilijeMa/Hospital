using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

public class FunctionalItem
{
	string Where { get; set; }
	string What { get; set; }
    int Amount { get; set; }
    public FunctionalItem(string room, string equipment, int amount)
	{
		this.Where = room;
		this.What = equipment;
		this.Amount = amount;
	}
    public static List<FunctionalItem> LoadAll()
    {

        var serializer = new JsonSerializer();
        using StreamReader reader = new("./../../../data/functionalItems.json");
        var json = reader.ReadToEnd();
        List<FunctionalItem> allFunctionalItems = JsonConvert.DeserializeObject<List<FunctionalItem>>(json);
        return allFunctionalItems;
    }

    public string ToString()
    {
        return What + " " + Where + " " + Amount.ToString();
    }
}
