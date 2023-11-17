namespace BattleEvents
{
    public class HealEvent : BattleEvent
    {
        public int LifeRestored { get; }
        public Element Element { get; }

        public HealEvent(int lifeRestored, Element element)
        {
            LifeRestored = lifeRestored;
            Element = element;
        }
    }
}
