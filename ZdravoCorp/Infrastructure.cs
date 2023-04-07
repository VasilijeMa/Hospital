using System;
using System.Collections.Generic;

public class Infrastructure
{
    protected Dictionary<Equipment, int> Inventory;
    public Infrastructure(Dictionary<Equipment, int> inventory)
	{
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
