namespace _Project.Codebase.Gameplay.Characters
{
    public interface IDamageable
    {
        public int Health { get; }
        public int MaxHealth { get; }

        public void TakeDamage(int damage);
    }
}