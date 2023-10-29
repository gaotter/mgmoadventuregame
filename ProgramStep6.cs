using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Step6
{

    class Room
    {
        public string Name;
        public string Description;
        public Dictionary<string, Room> Connections;
        public int NumEnemies;
        public int NumCoins;
        public bool HasStaffOfHope;
        public List<Enemy> Enemies { get; set; } = new List<Enemy>();

        public Room(string name, string description, int numCoins, bool hasStaffOfHope)
        {
            Name = name;
            Description = description;
            Connections = new Dictionary<string, Room>();
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

        public void AddEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
            string enemyDescription = string.Format("You see a {0} with {1} health.", enemy.Name.ToLower(), enemy.Health);
            // Description += " " + enemyDescription;
        }

        public void RemoveEnemy(Enemy enemy)
        {
            Enemies.Remove(enemy);
            string enemyDescription = string.Format("You defeated the {0}!", enemy.Name.ToLower());
            //  Description = Description.Replace(enemyDescription, "");
        }

    }

    class Game
    {
        private static Player player = new Player("Aragorn", 1500, 400);
        public static void Main(string[] args)
        {

            Console.WriteLine(@"The world is in peril. The evil demon Surus has unleashed his wrath upon the land, and the only way to stop him is to destroy him with the Staff of Hope. The staff was created by a forgotten wizard guild that once stood against Surus. But the guild was destroyed by Surus, and the staff was lost.

You play as the hero who must enter the ruins of the wizard guild to retrieve the staff. The ruins are filled with traps and puzzles that you must solve to progress. Once you have the staff, you must journey to Surus’s lair and defeat him once and for all.

Good luck on your quest! You enter your first room.");

            // Define the game world
            Room starterRoom = new Room("Starter Room", "(S) The room is dark and damp, with an unpleasant smell of mold and moisture. The walls are covered in greenish mold, and there are water stains on the ceiling. The floor is made of old creaky wood that makes noise under your feet, and there are damp spots on the floor. ", 0, false);
            Room[] randomRooms = {
            new Room("Room 0", "(R0) The room is dimly lit, with candles flickering on the walls and a large fireplace at one end of the room. The fireplace is made of stone, and there is a large armchair next to it where the wizard would sit and read.", 50, false),
            new Room("Room 1", "(R1) The centerpiece of the room is a large wooden table, surrounded by high-backed chairs. The table is covered in a white tablecloth, with silver candelabras and crystal goblets set at each place setting. The plates are made of fine china, with intricate designs etched into them.", 100, false),
            new Room("Room 2", "(R2) In the center of the room is large four-poster beds with a canopy. The beds are covered in soft blankets and pillows, and there is a small nightstands next to them with lamps and books. There is also a large rug on the floor, which adds to the cozy atmosphere of the room.", 25, false),
            new Room("Room 3", "(R3) There are several workstations around the room, each with its own set of tools and equipment. There are cauldrons bubbling away on the fireplaces, and jars of strange ingredients lining the shelves.", 200, false),
            new Room("Room 4", "(R4) The walls are adorned with the heads of various beasts that the wizards had hunted. However, all the beasts have been dead for a long time, and their fur has lost its luster. There is a large griffin head mounted above the fireplace, and a unicorn head on the opposite wall.", 150, false)
        };
            Room winnerRoom = new Room("Winner Room", @"As you enter the room, you feel a rush of magic energy fill your body. The room is filled with a bright light, and you can feel the power of the staff coursing through your veins.
You are now ready to face the evil demon Surus. You can feel the magic of the staff empowering you, giving you the strength and courage to face any challenge that comes your way.
As you leave the room, you can feel the power of the staff still coursing through your body. You know that with this staff in your hands, you have a chance to defeat Surus and save the world from his evil grasp.
 .You have found the Staff of Hope! Congratulations!", 0, true);

            // Connect the starter room to random rooms
            starterRoom.AddConnection("east", randomRooms[1]);

            randomRooms[1].AddConnection("east", randomRooms[2]);
            randomRooms[1].AddConnection("south", randomRooms[0]);

            randomRooms[1].AddEnemy(new Enemy("Goblin", 10, 5));
            randomRooms[1].AddEnemy(new Enemy("Goblin", 10, 5));
            
            

            randomRooms[2].AddConnection("south", randomRooms[3]);

            randomRooms[2].AddEnemy(new Enemy("Orc", 100, 100));
            randomRooms[2].AddEnemy(new Enemy("Goblin", 10, 5));
            randomRooms[2].AddEnemy(new Enemy("Goblin", 10, 5));

            randomRooms[3].AddConnection("west", randomRooms[0]);

            randomRooms[4].AddEnemy(new Enemy("Dragon", 1000, 450));

            randomRooms[0].AddConnection("west", randomRooms[4]);
            randomRooms[0].AddConnection("south", winnerRoom);

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
            if (currentRoom.Enemies.Count > 0)
            {
                Console.WriteLine("You are under attack!");

                // Create a list of all combatants (player and enemies)
                List<object> combatants = new List<object>();
                combatants.Add(player);
                combatants.AddRange(currentRoom.Enemies);

                foreach (var combatant in combatants)
                {
                    if (combatant is Enemy)
                    {
                        Console.WriteLine($"You see a {((Enemy)combatant).Name}");
                    }
                }

                // Enter combat mode
                while (true)
                {
                    // Print the health of all combatants
                    Console.WriteLine("Combatants:");
                    foreach (object combatant in combatants)
                    {
                        if (combatant is Player)
                        {
                            Console.WriteLine("{0}: {1} health", ((Player)combatant).Name, ((Player)combatant).Health);
                        }
                        else if (combatant is Enemy)
                        {
                            Console.WriteLine("{0}: {1} health", ((Enemy)combatant).Name, ((Enemy)combatant).Health);
                        }
                    }

                    // Get the player's input
                    Console.Write("What do you want to do? ");
                    string input = Console.ReadLine().ToLower();

                    // Check if the input is valid
                    if (input == "attack")
                    {
                        Random random = new Random();
                        // Player attacks a random enemy
                        Enemy enemy = currentRoom.Enemies[random.Next(currentRoom.Enemies.Count)];
                        player.Attack(enemy);
                        if (enemy.Health <= 0)
                        {
                            currentRoom.RemoveEnemy(enemy);
                        }

                        // Enemies attack the player
                        foreach (Enemy enemy2 in currentRoom.Enemies)
                        {
                            enemy2.Attack(player);
                        }

                        // Check if the player is dead
                        if (player.Health <= 0)
                        {
                            Console.WriteLine("You have been defeated!");
                            return;
                        }

                        // Check if all enemies are dead
                        if (currentRoom.Enemies.Count == 0)
                        {
                            Console.WriteLine("You have defeated all enemies!");
                            break;
                        }
                    }
                    else if (input == "run")
                    {
                        Console.WriteLine("You run away!");
                        break;
                    }
                    else if (input == "look")
                    {
                        var enemyType = combatants.Select(c =>
                        {
                            if (c is Enemy)
                            {
                                return ((Enemy)c).Name;
                            }
                            return "";
                        });

                        foreach (var enemy in enemyType)
                        {
                            if (enemy.Equals("Goblin"))
                            {
                                PrintGobling();
                            }

                            if(enemy.Equals("Orc"))
                            {
                                PrintOrc();
                            }

                            if(enemy.Equals("Dragon"))
                            {
                                PrintDragon();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unknown command.");
                    }
                }
               

            }

            // Check if the player has found coins
            if (currentRoom.NumCoins > 0)
            {
                Console.WriteLine("You have found {0} coins!", currentRoom.NumCoins);
                // TODO: Implement inventory system
            }

            // Get the player's input
            Console.Write("What do you want to do? ");
            string input2 = Console.ReadLine();

            if (input2 == "see map")
            {
                PrintMap();
                PlayGame(currentRoom);
            }
            else if (currentRoom.Connections.ContainsKey(input2))
            {
                Room nextRoom = currentRoom.Connections[input2];
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
            Console.WriteLine("| ? | --- | R0|-----| R3|");
            Console.WriteLine("+---+     +---+     +---+");
            Console.WriteLine("            |");
            Console.WriteLine("         +---+");
            Console.WriteLine("         | ? |");
            Console.WriteLine("         +---+");
        }

        static void PrintGobling()
        {
            Console.WriteLine(@"
                ,      ,
               /(.-""-.)\
           |\  \/      \/  /|
           | \ / =.  .= \ / |
           \( \   o\/o   / )/
            \_, '-/  \-' ,_/
              /   \__/   \
              \ \__/\__/ /
            ___\ \|--|/ /___
          /`    \      /    `\
         /       '----'       \
");
        }


        static void PrintOrc()
        {
            Console.WriteLine(@"
               __,__
      \    /\\_//\
       \\_/_\_\/_/_/
      __\_\/\_\/_/__
     / /\\_\_\_\//\\\
     \/\\__\_\_\/\\/
      \\/_/\/\_\\_\/
       \_\_\||/_/_/
        \/ \/ \/ \/
");
        }


         static void PrintDragon()
        {
            Console.WriteLine(@"
               / \__
              (    @\___
              /         O
             /   (_____/
            /_____/   U
");
        }
    }
    


    public class Enemy
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }

        public Enemy(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
        }

        public void Attack(Player player)
        {
            Console.WriteLine("{0} attacks {1} for {2} damage!", Name, player.Name, Damage);
            player.Health -= Damage;
        }
    }


    public class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }

        public Player(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
        }

        public void Attack(Enemy enemy)
        {
            Console.WriteLine("{0} attacks {1} for {2} damage!", Name, enemy.Name, Damage);
            enemy.Health -= Damage;
        }
    }

}