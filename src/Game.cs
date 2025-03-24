using System;
using System.Text;
using System.Collections.Generic;
using System.Transactions;
using System.Net.Quic;

class Game
{
    // Private fields
    private Parser parser;
    private Player player;

    // Constructor
    public Game()
    {
        parser = new Parser();
        player = new Player();
        CreateRooms();
    }

    // Initialise the Rooms (and the Items)
    private void CreateRooms()
    {
        // Create the rooms
        Room outside = new Room("outside the main entrance of the university");
        Room theatre = new Room("in a lecture theatre");
        Room pub = new Room("in the campus pub");
        Room lab = new Room("in a computing lab");
        Room basement = new Room("A basement under the lab");
        Room office = new Room("in the computing admin office");

        // Initialise room exits
        outside.AddExit("east", theatre);
        outside.AddExit("south", lab);
        outside.AddExit("west", pub);

        theatre.AddExit("west", outside);

        pub.AddExit("east", outside);

        lab.AddExit("north", outside);
        lab.AddExit("east", office);
        lab.AddExit("down", basement);

        office.AddExit("west", lab);

        basement.AddExit("up", lab);

        //items

        Item bandage = new Item(5, "Small bandage to heal yourself 50hp");
        Item bandage2 = new Item(5, "Small bandage to heal yourself 50hp");
        Item pill = new Item(1, "Immunity to damage when you move");
        Item medkit = new Item(10, "Medkit that stops bleeding and gets you back to 100hp");
        Item key = new Item(10, "Used to open a door");



        //Items to rooms

        basement.Chest.Put("bandage", bandage);
        pub.Chest.Put("bandage", bandage2);
        lab.Chest.Put("superPill", pill);
        office.Chest.Put("medkit", medkit);
        theatre.Chest.Put("key", key);


        // Start game outside
        player.CurrentRoom = outside;
    }

    // Main play routine. Loops until end of play. !!! ADD MORE!!
    public void Play()
    {
        PrintWelcome();

        // Enter the main command loop. Here we repeatedly read commands and
        // execute them until the player wants to quit.
        bool finished = false;
        while (!finished)
        {
            Command command = parser.GetCommand();
            finished = ProcessCommand(command);
            if (player.Health == 0)
            {
                Console.WriteLine("is dead");
                finished = true;
            }
        }

        Console.WriteLine("Thank you for playing.");
        Console.WriteLine("Press [Enter] to continue.");
        Console.ReadLine();
    }

    // Print out the opening message for the player.
    private void PrintWelcome()
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to Zuul!");
        Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
        Console.WriteLine("Type 'help' if you need help.");
        Console.WriteLine();
        Console.WriteLine(player.CurrentRoom.GetLongDescription());
        Console.WriteLine("Items in the room: {0}", player.CurrentRoom.Chest.Show());

    }

    // Given a command, process (that is: execute) the command.
    // If this command ends the game, it returns true.
    // Otherwise false is returned.
    private bool ProcessCommand(Command command)
    {
        bool wantToQuit = false;

        if (command.IsUnknown())
        {
            Console.WriteLine("I don't know what you mean...");
            return wantToQuit; // false
        }

        switch (command.CommandWord)
        {
            case "help":
                PrintHelp();
                break;
            case "go":
                GoRoom(command);
                break;
            case "quit":
                wantToQuit = true;
                break;
            case "look":
                Console.WriteLine(player.CurrentRoom.GetLongDescription());
                break;
            case "status":
                PrintStatus();
                break;
            case "take":
                player.TakeFromChest(command.SecondWord);
                break;
            case "use":
                player.use(command.SecondWord);
                break;
        }

        return wantToQuit;
    }

    // Implementations of user commands:

    // Print out some help information.
    private void PrintHelp()
    {
        Console.WriteLine("You are lost. You are alone.");
        Console.WriteLine("You wander around at the university.");
        Console.WriteLine();
        // Let the parser print the commands
        parser.PrintValidCommands();
    }
    private void PrintStatus()
    {
        Console.WriteLine($"Health: {player.Health}");

        // Display items in the player's inventory
        Console.WriteLine(player.Inventory.Show());

    }
    // Try to go to one direction. If there is an exit, enter the new
    // room, otherwise print an error message.
    private void GoRoom(Command command)
    {
        if (!command.HasSecondWord())
        {
            // If there is no second word, we don't know where to go...
            Console.WriteLine("Go where?");
            return;
        }

        string direction = command.SecondWord;

        // Try to go to the next room.
        Room nextRoom = player.CurrentRoom.GetExit(direction);
        if (nextRoom == null)
        {
            Console.WriteLine("There is no door to " + direction + "!");
            return;
        }

        player.CurrentRoom = nextRoom;
        player.Damage(10);
        Console.WriteLine($"your health is: {player.Health}");
        Console.WriteLine(player.CurrentRoom.GetLongDescription());
        Console.WriteLine("Items in the room: {0}", player.CurrentRoom.Chest.Show());
    }
}
