using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

class Player
{

    // fields
    public Inventory Inventory { get { return backpack; } }
    private int health;
    private Inventory backpack;

    // Public property to access health
    public int Health
    {
        get { return health; }
    }

    // auto property
    public Room CurrentRoom { get; set; }
    // constructor
    public Player()
    {
        backpack = new Inventory(40);
        health = 100;
        CurrentRoom = null;
    }
    //methods
    public void Damage(int amount)
    {
        // Reduce the player's health by the specified amount
        health -= amount;
        // Ensure health doesn't go below 0
        if (health < 0)
        {
            health = 0;
        }
    }

    public void Heal(int amount)
    {
        // Increase the player's health by the specified amount
        health += amount;
        // Ensure health doesn't exceed 100
        if (health > 100)
        {
            health = 100;
        }
    }

    public bool IsAlive()
    {
        // Check if the player's health is greater than 0
        return health > 0;
    }
    public bool IsWounded()
    {
        // Check if the player's health is less than 100
        return health < 100;
    }
    public bool TakeFromChest(string itemName)
    {
        Inventory chestInventory = CurrentRoom.Chest;

        if (chestInventory != null)
        {
            // Attempt to get the item from the chest
            Item item = chestInventory.Get(itemName);

            // Check if the item was successfully retrieved
            if (item != null)
            {
                // Attempt to put the item into the player's inventory
                bool success = backpack.Put(itemName, item);

                if (success)
                {
                    Console.WriteLine($"You took {itemName} from the chest.");
                    return true; // Successfully took the item from the chest
                }
                else
                {
                    // If the item couldn't fit in the player's inventory, put it back in the chest
                    chestInventory.Put(itemName, item);
                    Console.WriteLine($"You couldn't carry {itemName} and left it in the chest.");
                    return false; // Failed to take the item due to lack of space in the player's inventory
                }
            }
            else
            {
                // If the item was not found in the chest
                Console.WriteLine($"There is no {itemName} in the chest.");
                return false; // Failed to take the item from the chest
            }
        }
        else
        {
            // If the chest inventory is null (not available)
            Console.WriteLine("The chest is not available.");
            return false; // Failed to take the item from the chest
        }
    }
    public bool DropToChest(string itemName)
    {
        Inventory chestInventory = CurrentRoom.Chest;

        if (chestInventory != null)
        {
            // Attempt to get the item from the backpack
            Item item = backpack.Get(itemName);

            // Check if the item is known
            if (item != null)
            {
                // Attempt to put the item back in the chest
                bool success = chestInventory.Put(itemName, item);

                if (success)
                {
                    Console.WriteLine($"You put {itemName} in the chest.");
                    return true; // Successfully put the item back in the chest
                }
                else
                {
                    Console.WriteLine($"Failed to put {itemName} in the chest. Inventory full?");
                    return false; // Failed to put the item back in the chest (possibly due to inventory full)
                }
            }
            else
            {
                // If the item was not found in the backpack
                Console.WriteLine($"You do not have a {itemName} in your inventory.");
                return false; // Failed to put the item back in the chest (item not found in backpack)
            }
        }
        else
        {
            // If the chest inventory is not available
            Console.WriteLine("The chest is not available.");
            return false; // Failed to put the item back in the chest (chest not available)
        }
    }

    public void use(string Itemname)
    {
        Item currentItem = backpack.Get(Itemname);
        if (currentItem != null)
        {
            if (Itemname == "medkit")
            {
                Heal(50);
            }

            else if (Itemname == "bandage")
            {
                Heal(15);
            }

            else if (Itemname == "pill")
            {
                Heal(70);
            
        }
    }
}

}