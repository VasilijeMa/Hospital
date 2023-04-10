using System;

public class FunctionalItem
{
	Room Where;
	Equipment What;
	int Amount;
	public FunctionalItem(Room room, Equipment equipment, int amount)
	{
		this.Where = room;
		this.What = equipment;
		this.Amount = amount;
	}
}
