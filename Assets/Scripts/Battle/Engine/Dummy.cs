using System;
using System.Collections.Generic;
using System.Linq;
using BattleEvents;

public class Dummy : Creature
{
    public int MinDamage { get; private set; }
    public int MaxDamage { get; private set; }

    public Dummy(string name, int maxHealth, Element element,
        int minDamage, int maxDamage) : base(name, maxHealth, element)
    {
        MinDamage = minDamage;
        MaxDamage = maxDamage;
    }

    public override IEnumerable<BattleEvent> Act()
    {
        battle.Logger.Log($"{Name} attacks you!");

        yield return new MoveEvent(this);

        int damage = battle.Rand.Next(MinDamage, MaxDamage + 1);
        Attack attack = new Attack(damage, Element, new List<string>());
        BattleEvent ev = battle.Witch.Hurt(attack);
        yield return ev;
    }

    public override BattleEvent Hurt(Attack attack)
    {
        Tuple<int, int> dmgTaken = GetDamageTaken(attack);
        int damage = dmgTaken.Item1;
        int reactionType = dmgTaken.Item2;
        string dmgType = attack.Tags.First();
        Health = Math.Max(0, Health - damage);
        battle.Logger.Log($"{Name} took {damage} damage!");
        return new DamageEvent(this, damage, attack.Element, reactionType, dmgType);
    }
}
