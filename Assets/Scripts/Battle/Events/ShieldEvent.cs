namespace BattleEvents
{
    public class ShieldEvent : BattleEvent
    {
        public Shield Shield { get; }

        public ShieldEvent(Shield shield)
        {
            Shield = shield;
        }
    }
}
