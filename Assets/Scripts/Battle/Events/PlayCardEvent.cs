namespace BattleEvents
{
    public class PlayCardEvent : BattleEvent
    {
        public Card Card { get; }

        public PlayCardEvent(Card card)
        {
            Card = card;
        }
    }
}
