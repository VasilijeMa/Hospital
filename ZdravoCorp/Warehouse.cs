using System;
using System.Collections.Generic;

public class Warehouse:Infrastructure
{
    public Warehouse(Dictionary<Equipment, int> inventory) : base(inventory)
    {
    }
    
}
