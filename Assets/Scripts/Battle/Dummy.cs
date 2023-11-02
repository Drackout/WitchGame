using System;
using System.Collections.Generic;
using BattleEvents;

public class Dummy : Creature
{
    public Dummy(string name, int maxHealth, Element element) : base(name, maxHealth, element)
    {
    }

    public override IEnumerable<BattleEvent> Act()
    {
        battle.Logger.Log($"{Name} attacks you!");
        BattleEvent ev = battle.Witch.Hurt(new Attack(2, Element.None, new List<string>()));
        yield return ev;
    }

    public override BattleEvent Hurt(Attack attack)
    {
        int damage = DamageTaken(attack);
        Health = Math.Max(0, Health - damage);
        battle.Logger.Log($"{Name} took {damage} damage!");
        return new DamageEvent(this, damage);
    }
}
