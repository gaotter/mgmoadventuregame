namespace Step1
{
    /// <summary>
    /// 100% generetet av co pilot. Fungerer men er ikke så lett å spille, siden man må skrive inn hele romnavnet for å gå til neste rom.
    /// </summary>
    class Game
    {
        public static void Main(string[] args)
       {
        // Define the game world
        Room starterRoom = new Room("Starter Room", "You are in a small room. There is a door to the north.", 0, 0, false);
        Room[] randomRooms = {
            new Room("Random Room 1", "You are in a dark room. There is a door to the north.", 2, 50, false),
            new Room("Random Room 2", "You are in a dusty room. There is a door to the east.", 3, 100, false),
            new Room("Random Room 3", "You are in a damp room. There is a door to the west.", 1, 25, false),
            new Room("Random Room 4", "You are in a cold room. There is a door to the south.", 0, 200, false),
            new Room("Random Room 5", "You are in a hot room. There is a door to the east.", 5, 150, false)
        };
        Room winnerRoom = new Room("Winner Room", "You have found the Staff of Hope! Congratulations!", 0, 0, true);

        // Connect the starter room to random rooms
        ConnectRooms(starterRoom, randomRooms);

        // Connect random rooms to other random rooms or the winner room
        ConnectRooms(randomRooms[0], new Room[] { randomRooms[1], randomRooms[2], winnerRoom });
        ConnectRooms(randomRooms[1], new Room[] { randomRooms[0], randomRooms[3], winnerRoom });
        ConnectRooms(randomRooms[2], new Room[] { randomRooms[0], randomRooms[4], winnerRoom });
        ConnectRooms(randomRooms[3], new Room[] { randomRooms[1], winnerRoom });
        ConnectRooms(randomRooms[4], new Room[] { randomRooms[2], winnerRoom });

        // Start the game in the starter room
        PlayGame(starterRoom);
    }

    static void ConnectRooms(Room room, Room[] connections) {
        foreach (Room connection in connections) {
            room.Connections.Add(connection);
        }
    }

    static void ConnectRooms(Room room, List<Room> connections) {
        foreach (Room connection in connections) {
            room.Connections.Add(connection);
        }
    }

    static void PlayGame(Room currentRoom) {
        // Print the current room description
        Console.WriteLine(currentRoom.Description);

        // Check if the player has won
        if (currentRoom.HasStaffOfHope) {
            Console.WriteLine("You have won the game!");
            return;
        }

        // Check if the player has encountered enemies
        if (currentRoom.NumEnemies > 0) {
            Console.WriteLine("You have encountered {0} enemies!", currentRoom.NumEnemies);
            // TODO: Implement combat system
        }

        // Check if the player has found coins
        if (currentRoom.NumCoins > 0) {
            Console.WriteLine("You have found {0} coins!", currentRoom.NumCoins);
            // TODO: Implement inventory system
        }

        // Get the player's input
        Console.Write("What do you want to do? ");
        string input = Console.ReadLine();

        // Check if the input is valid
        Room nextRoom = null;
        foreach (Room connection in currentRoom.Connections) {
            if (connection.Name.ToLower().Contains(input.ToLower())) {
                nextRoom = connection;
                break;
            }
            // Console.WriteLine("rooms: {0}", connection.Name);
        }
        if (nextRoom == null) {
            Console.WriteLine("You can't do that!");
            PlayGame(currentRoom);
        } else {
            PlayGame(nextRoom);
        }
    }

    }


    class Room
    {
        public string Name;
        public string Description;
        public List<Room> Connections;
        public int NumEnemies;
        public int NumCoins;
        public bool HasStaffOfHope;

        public Room(string name, string description, int numEnemies, int numCoins, bool hasStaffOfHope)
        {
            Name = name;
            Description = description;
            Connections = new List<Room>();
            NumEnemies = numEnemies;
            NumCoins = numCoins;
            HasStaffOfHope = hasStaffOfHope;
        }
    }



}
