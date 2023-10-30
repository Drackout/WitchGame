namespace BattleEvents
{
    public class DamageEvent : BattleEvent
    {
        public int Damage { get; }
        public Battler Target { get; }

        public DamageEvent(Battler target, int damage)
        {
            Target = target;
            Damage = damage;
        }
    }
}
