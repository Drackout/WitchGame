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
        yield return new MoveEvent(this);
        BattleEvent ev = battle.Witch.Hurt(new Attack(2, Element, new List<string>()));
        yield return ev;
    }

    public override BattleEvent Hurt(Attack attack)
    {
        Tuple<int, int> dmgTaken = GetDamageTaken(attack);
        int damage = dmgTaken.Item1;
        int reactionType = dmgTaken.Item2;
        Health = Math.Max(0, Health - damage);
        battle.Logger.Log($"{Name} took {damage} damage!");
        return new DamageEvent(this, damage, attack.Element, reactionType);
    }
}
