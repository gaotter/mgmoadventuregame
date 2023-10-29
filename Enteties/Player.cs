namespace Mgmoadventuregame.Enteties
{
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