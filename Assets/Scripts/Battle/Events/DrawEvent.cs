namespace BattleEvents
{
    public class DrawEvent : BattleEvent
    {
        public Card Card { get; }

        public DrawEvent(Card card)
        {
            Card = card;
        }
    }
}
