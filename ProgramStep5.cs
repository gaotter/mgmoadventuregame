using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Step5
{

    class Room
    {
        public string Name;
        public string Description;
        public Dictionary<string, Room> Connections;
        public int NumEnemies;
        public int NumCoins;
        public bool HasStaffOfHope;

        public Room(string name, string description, int numEnemies, int numCoins, bool hasStaffOfHope)
        {
            Name = name;
            Description = description;
            Connections = new Dictionary<string, Room>();
            NumEnemies = numEnemies;
            NumCoins = numCoins;
            HasStaffOfHope = hasStaffOfHope;
        }

        public void AddConnection(string direction, Room room)
        {
            if (Connections.ContainsKey(direction))
            {
                throw new ArgumentException("Direction already used by another room.");
            }

            Connections[direction] = room;
            string oppositeDirection = GetOppositeDirection(direction);
            room.Connections[oppositeDirection] = this;
            string connectionDescription = string.Format("You see a door to the {1}.", room.Name.ToLower(), direction);
            Description += " " + connectionDescription;
            room.Description += " " + GetOppositeDirectionDescription(connectionDescription);
        }

        private string GetOppositeDirection(string direction)
        {
            switch (direction)
            {
                case "north":
                    return "south";
                case "east":
                    return "west";
                case "south":
                    return "north";
                case "west":
                    return "east";
                default:
                    throw new ArgumentException("Invalid direction: " + direction);
            }
        }

        private string GetOppositeDirectionDescription(string connectionDescription)
        {
            string pattern = @"\b(north|east|south|west)\b";
            Match match = Regex.Match(connectionDescription, pattern);
            if (match.Success)
            {
                string direction = match.Groups[1].Value;
                string oppositeDirection = GetOppositeDirection(direction);
                return Regex.Replace(connectionDescription, pattern, oppositeDirection);
            }
            else
            {
                throw new ArgumentException("Invalid connection description: " + connectionDescription);
            }
        }

        // first bug
        // private string GetOppositeDirectionDescription(string connectionDescription)
        // {
        //     string[] words = connectionDescription.Split(' ');
        //     string oppositeDirection = GetOppositeDirection(words[3].TrimEnd('.'));
        //     words[3] = oppositeDirection + ".";
        //     return string.Join(" ", words);
        // }

    }

    class Game
    {
        public static void Main(string[] args)
        {

Console.WriteLine(@"The world is in peril. The evil demon Surus has unleashed his wrath upon the land, and the only way to stop him is to destroy him with the Staff of Hope. The staff was created by a forgotten wizard guild that once stood against Surus. But the guild was destroyed by Surus, and the staff was lost.

You play as the hero who must enter the ruins of the wizard guild to retrieve the staff. The ruins are filled with traps and puzzles that you must solve to progress. Once you have the staff, you must journey to Surus’s lair and defeat him once and for all.

Good luck on your quest! You enter your first room.");

            // Define the game world
            Room starterRoom = new Room("Starter Room", "The room is dark and damp, with an unpleasant smell of mold and moisture. The walls are covered in greenish mold, and there are water stains on the ceiling. The floor is made of old creaky wood that makes noise under your feet, and there are damp spots on the floor. ", 0, 0, false);
            Room[] randomRooms = {
            new Room("Room 0", "The room is dimly lit, with candles flickering on the walls and a large fireplace at one end of the room. The fireplace is made of stone, and there is a large armchair next to it where the wizard would sit and read.", 2, 50, false),
            new Room("Room 1", "The centerpiece of the room is a large wooden table, surrounded by high-backed chairs. The table is covered in a white tablecloth, with silver candelabras and crystal goblets set at each place setting. The plates are made of fine china, with intricate designs etched into them.", 3, 100, false),
            new Room("Room 2", "In the center of the room is large four-poster beds with a canopy. The beds are covered in soft blankets and pillows, and there is a small nightstands next to them with lamps and books. There is also a large rug on the floor, which adds to the cozy atmosphere of the room.", 1, 25, false),
            new Room("Room 3", "There are several workstations around the room, each with its own set of tools and equipment. There are cauldrons bubbling away on the fireplaces, and jars of strange ingredients lining the shelves.", 0, 200, false),
            new Room("Room 4", "The walls are adorned with the heads of various beasts that the wizards had hunted. However, all the beasts have been dead for a long time, and their fur has lost its luster. There is a large griffin head mounted above the fireplace, and a unicorn head on the opposite wall.", 5, 150, false)
        };
            Room winnerRoom = new Room("Winner Room", @"As you enter the room, you feel a rush of magic energy fill your body. The room is filled with a bright light, and you can feel the power of the staff coursing through your veins.
You are now ready to face the evil demon Surus. You can feel the magic of the staff empowering you, giving you the strength and courage to face any challenge that comes your way.
As you leave the room, you can feel the power of the staff still coursing through your body. You know that with this staff in your hands, you have a chance to defeat Surus and save the world from his evil grasp.
 .You have found the Staff of Hope! Congratulations!", 0, 0, true);

            // Connect the starter room to random rooms
            starterRoom.AddConnection("east", randomRooms[1]);

            randomRooms[1].AddConnection("east", randomRooms[2]);
            randomRooms[1].AddConnection("south", randomRooms[0]);

            randomRooms[2].AddConnection("south", randomRooms[3]);

            randomRooms[3].AddConnection("west", randomRooms[0]);

            randomRooms[0].AddConnection("west", randomRooms[4]);
            randomRooms[0].AddConnection("south", winnerRoom);

            // Connect random rooms to other random rooms or the winner room
            // randomRooms[0].AddConnection("east", randomRooms[1]);
            // randomRooms[0].AddConnection("south", randomRooms[2]);
            // randomRooms[0].AddConnection("west", randomRooms[3]);
            // //  randomRooms[1].AddConnection("west", randomRooms[0]);
            // randomRooms[1].AddConnection("south", randomRooms[4]);
            // randomRooms[2].AddConnection("north", winnerRoom);
            // randomRooms[2].AddConnection("east", randomRooms[4]);
            // // randomRooms[3].AddConnection("east", randomRooms[0]);
            // randomRooms[3].AddConnection("south", randomRooms[4]);
            // // randomRooms[4].AddConnection("north", randomRooms[1]);
            // //randomRooms[4].AddConnection("west", randomRooms[2]);
            //randomRooms[4].AddConnection("north", winnerRoom);



            // Connect random rooms to other random rooms or the winner room
            // will cause a bug
            // randomRooms[0].AddConnection("north", randomRooms[1]);
            // randomRooms[0].AddConnection("east", randomRooms[2]);
            // randomRooms[0].AddConnection("west", winnerRoom);
            // randomRooms[1].AddConnection("west", randomRooms[0]);
            // randomRooms[1].AddConnection("south", randomRooms[3]);
            // randomRooms[1].AddConnection("east", winnerRoom);
            // randomRooms[2].AddConnection("west", randomRooms[0]);
            // randomRooms[2].AddConnection("south", randomRooms[4]);
            // randomRooms[2].AddConnection("east", winnerRoom);
            // randomRooms[3].AddConnection("north", randomRooms[1]);
            // randomRooms[3].AddConnection("west", winnerRoom);
            // randomRooms[4].AddConnection("north", randomRooms[2]);
            // randomRooms[4].AddConnection("west", winnerRoom);

            // Start the game in the starter room
            PlayGame(starterRoom);
        }

        static void PlayGame(Room currentRoom)
        {
            // Print the current room description
            Console.WriteLine(currentRoom.Description);


            // Check if the player has won
            if (currentRoom.HasStaffOfHope)
            {
                Console.WriteLine("You have won the game!");
                return;
            }

            // Check if the player has encountered enemies
            if (currentRoom.NumEnemies > 0)
            {
                Console.WriteLine("You have encountered {0} enemies!", currentRoom.NumEnemies);
                // TODO: Implement combat system
            }

            // Check if the player has found coins
            if (currentRoom.NumCoins > 0)
            {
                Console.WriteLine("You have found {0} coins!", currentRoom.NumCoins);
                // TODO: Implement inventory system
            }

            // Get the player's input
            Console.Write("What do you want to do? ");
            string input = Console.ReadLine();

            if (input == "see map")
            {
                PrintMap();
                PlayGame(currentRoom);
            }
            else if (currentRoom.Connections.ContainsKey(input))
            {
                Room nextRoom = currentRoom.Connections[input];
                PlayGame(nextRoom);
            }
            else
            {
                Console.WriteLine("You can't do that!");
                PlayGame(currentRoom);
            }
        }

        static void PrintMap()
        {
            Console.WriteLine("+---+     +---+     +---+");
            Console.WriteLine("| S | --- | R1|-----| R2|");
            Console.WriteLine("+---+     +---+     +---+");
            Console.WriteLine("            |         |");
            Console.WriteLine("+---+     +---+     +---+");
            Console.WriteLine("| R4| --- | R0|-----| R3|");
            Console.WriteLine("+---+     +---+     +---+");
            Console.WriteLine("            |");
            Console.WriteLine("         +---+");
            Console.WriteLine("         | W |");
            Console.WriteLine("         +---+");
        }
    }

}