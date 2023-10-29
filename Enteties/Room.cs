using System.Text.RegularExpressions;

namespace Mgmoadventuregame.Enteties
{
    public class Room
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
}