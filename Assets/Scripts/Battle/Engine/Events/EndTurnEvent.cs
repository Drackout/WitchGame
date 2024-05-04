using BattleEvents;

public class EndTurnEvent : BattleEvent
{
    public Battler Battler { get; set; }

    public EndTurnEvent(Battler battler)
    {
        Battler = battler;
    }
}
