namespace BattleEvents
{
    public class InputRequestEvent : BattleEvent
    {
        public InputRequestType Type { get; }

        public InputRequestEvent(InputRequestType type)
        {
            Type = type;
        }
    }
}
