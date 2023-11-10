using BattleEvents;

public class SlotsEvent : BattleEvent
{
    public int Previous { get; }
    public int Current { get; }

    public SlotsEvent(int previous, int current)
    {
        Previous = previous;
        Current = current;
    }
}
