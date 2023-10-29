namespace Mgmoadventuregame.Enteties
{
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
}