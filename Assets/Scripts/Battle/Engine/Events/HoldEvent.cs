using BattleEvents;

public class HoldEvent : BattleEvent
{
    public int HeldCard { get; private set; }
    public bool Success { get; private set; }

    public HoldEvent(int index, bool success)
    {
        HeldCard = index;
        Success = success;
    }
}
