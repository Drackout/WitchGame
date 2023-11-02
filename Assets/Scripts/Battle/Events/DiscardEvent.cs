namespace BattleEvents
{
    public class DiscardEvent : BattleEvent
    {
        public Card Card { get; }
        public int Index { get; }

        public DiscardEvent(Card card, int index)
        {
            Card = card;
            Index = index;
        }
    }
}
