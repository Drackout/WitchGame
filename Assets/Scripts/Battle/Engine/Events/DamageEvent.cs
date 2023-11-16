namespace BattleEvents
{
    public class DamageEvent : BattleEvent
    {
        public int Damage { get; }
        public Battler Target { get; }
        public Element Element { get; }

        public DamageEvent(Battler target, int damage, Element element)
        {
            Target = target;
            Damage = damage;
            Element = element;
        }
    }
}
