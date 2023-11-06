namespace BattleEvents
{
    public class HealEvent : BattleEvent
    {
        public int LifeRestored { get; }

        public HealEvent(int lifeRestored)
        {
            LifeRestored = lifeRestored;
        }
    }
}
