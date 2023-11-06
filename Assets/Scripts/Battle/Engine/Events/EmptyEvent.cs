namespace BattleEvents
{
    public class EmptyEvent : BattleEvent
    {
        public string Warning { get; }

        public EmptyEvent(string warning)
        {
            Warning = warning;
        }
    }
}
