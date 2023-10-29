﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Step4
{
    /// <summary>
    /// Fungerende endret en del selv nytt kart. Navn på rom brukes ikke i besrivelsen av rommet.
    /// </summary>
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
            // Define the game world
            Room starterRoom = new Room("Starter Room", "You are in a small room", 0, 0, false);
            Room[] randomRooms = {
            new Room("Room 0", "You are in a dark room.", 2, 50, false),
            new Room("Room 1", "You are in a dusty room.", 3, 100, false),
            new Room("Room 2", "You are in a damp room.", 1, 25, false),
            new Room("Room 3", "You are in a cold room.", 0, 200, false),
            new Room("Room 4", "You are in a hot room.", 5, 150, false)
        };
            Room winnerRoom = new Room("Winner Room", "You have found the Staff of Hope! Congratulations!", 0, 0, true);

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