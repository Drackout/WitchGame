using System.Collections.Generic;
using BattleEvents;

public abstract class Battler
{
    public string Name { get; private set; }
    public int MaxHealth { get; protected set; }
    public int Health { get; protected set; }

    public Battle Battle
    {
        set
        {
            battle = value;
        }
    }

    protected Battle battle;

    public Battler(string name, int maxHealth)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = MaxHealth;
    }

    public abstract IEnumerable<BattleEvent> Act();

    public abstract BattleEvent Hurt(Attack attack);
}
