using System.Collections.Generic;
using System.Text;

class Room
{
	// Private fields
	private string description;
	private Dictionary<string, Room> exits; // stores exits of this room.'
	private Inventory chest;

	// Create a room described "description". Initially, it has no exits.
	// "description" is something like "in a kitchen" or "in a court yard".
	public Room(string desc)
	{
		chest = new Inventory(9999999);
		description = desc;
		exits = new Dictionary<string, Room>();
	}

	// Define an exit for this room.
	public void AddExit(string direction, Room neighbor)
	{
		exits.Add(direction, neighbor);
	}

	// Return the description of the room.
	public string GetShortDescription()
	{
		return description;
	}

	// Return a long description of this room, in the form:
	//     You are in the kitchen.
	//     Exits: north, west
public string GetLongDescription()
{
    StringBuilder str = new StringBuilder();
    str.Append("You are ");
    str.Append(description);
    str.AppendLine(".");
    str.Append(GetExitString());
    str.AppendLine();

    return str.ToString();
}


	// Return the room that is reached if we go from this room in direction
	// "direction". If there is no room in that direction, return null.
	public Room GetExit(string direction)
	{
		if (exits.ContainsKey(direction))
		{
			return exits[direction];
		}
		return null;
	}

	// Return a string describing the room's exits, for example
	// "Exits: north, west".
	private string GetExitString()
	{
		string str = "Exits:";

		// Build the string in a `foreach` loop.
		// We only need the keys.
		int countCommas = 0;
		foreach (string key in exits.Keys)
		{
			if (countCommas != 0)
			{
				str += ",";
			}
			str += " " + key;
			countCommas++;
		}

		return str;
	}
	public Inventory Chest
	{
		get { return chest; }
	}
public string GetItemsInRoom()
{
    StringBuilder itemString = new StringBuilder();
    itemString.AppendLine("Items in the room:");

    foreach (var item in chest.Items.Values)
    {
        itemString.AppendLine(item.Description);
    }

    return itemString.ToString();
}
	

}
