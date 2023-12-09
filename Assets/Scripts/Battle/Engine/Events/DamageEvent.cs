namespace BattleEvents
{
    public class DamageEvent : BattleEvent
    {
        public int Damage { get; }
        public Battler Target { get; }
        public Element Element { get; }
        public int ReactionType { get; }

        public DamageEvent(Battler target, int damage, Element element, int reactionType)
        {
            Target = target;
            Damage = damage;
            Element = element;
            ReactionType = reactionType;
        }
    }
}
