namespace BattleEvents
{
    public class MoveEvent : BattleEvent
    {
        public Battler Battler { get; }

        public MoveEvent(Battler battler)
        {
            Battler = battler;
        }
    }
}
