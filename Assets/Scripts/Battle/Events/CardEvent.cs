namespace BattleEvents
{
    public class CardEvent : BattleEvent
    {
        public Card Card { get; }

        public CardEvent(Card card)
        {
            Card = card;
        }
    }
}
