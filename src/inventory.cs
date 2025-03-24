using System;
using System.Collections.Generic;

class Inventory
{
    // fields
    private int maxWeight;
    private Dictionary<string, Item> items;
    // constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }
    // methods !!! FIX THIS !!!
    public bool Put(string itemName, Item item)
    {
        int weight = item.Weight;
        int leftover = FreeWeight();

        if (leftover >= weight)
        {
            // Add the item to the items dictionary
            items.Add(itemName, item);
            Console.WriteLine("Added " + itemName );
            return true; // Successfully put the item
        }

        return false; // Not enough free weight to put the item
    }

    public Item Get(string itemName)
    {
        // Check if the item exists in the items dictionary
        if (items.ContainsKey(itemName))
        {
            // Retrieve the item
            Item item = items[itemName];

            // Remove the item from the items dictionary
            items.Remove(itemName);

            // Return the retrieved item
            return item;
        }
        else
        {
            // Return null if the item doesn't exist in the dictionary
            return null;
        }
    }

    public int TotalWeight()
    {
        int total = 0;

        // Loop through the items in the items dictionary
        foreach (KeyValuePair<string, Item> kvp in items)
        {
            // Add the weight of each item to the total
            total += kvp.Value.Weight;
        }

        return total;
    }

    public int FreeWeight()
    {
        int leftover = maxWeight - TotalWeight();
        return leftover;
    }

    public string Show()
    {
        string inventory = "";

        foreach (KeyValuePair<string, Item> kvp in items)
        {
            inventory += kvp.Key;
        }
        return inventory;
    }
    public static string Show(Inventory inventory)
    {
        return inventory.Show();
    }

    public Dictionary<string, Item> Items
    {
        get { return items; }
    }


}