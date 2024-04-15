namespace BattleEvents
{
    public class PlayCardEvent : BattleEvent
    {
        public Card Card { get; }
        public int Index { get; }

        public PlayCardEvent(Card card, int index)
        {
            Card = card;
            Index = index;
        }
    }
}
