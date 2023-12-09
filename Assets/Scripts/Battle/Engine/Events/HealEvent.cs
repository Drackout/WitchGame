namespace BattleEvents
{
    public class HealEvent : BattleEvent
    {
        public int LifeRestored { get; }
        public Element Element { get; }
        public int ReactionType { get; }

        public HealEvent(int lifeRestored, Element element, int reactionType)
        {
            LifeRestored = lifeRestored;
            Element = element;
            ReactionType = reactionType;
        }
    }
}
