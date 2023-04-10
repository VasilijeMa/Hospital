using System;
using System.Collections.Generic;

public class Warehouse:Infrastructure
{
    Dictionary<Equipment, int> Inventory;
    public Warehouse(string name, Dictionary<Equipment, int> inventory) : base(name)
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
