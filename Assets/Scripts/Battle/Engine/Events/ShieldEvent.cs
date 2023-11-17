namespace BattleEvents
{
    public class ShieldEvent : BattleEvent
    {
        public Shield Shield { get; }
        public Element Element { get; }

        public ShieldEvent(Shield shield, Element element)
        {
            Shield = shield;
            Element = element;
        }
    }
}
