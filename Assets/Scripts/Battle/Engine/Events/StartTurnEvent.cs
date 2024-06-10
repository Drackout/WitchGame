using BattleEvents;

public class StartTurnEvent : BattleEvent
{
    public Battler Battler { get; }

    public StartTurnEvent(Battler battler)
    {
        Battler = battler;
    }
}
