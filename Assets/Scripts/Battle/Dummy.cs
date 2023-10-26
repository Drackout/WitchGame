using System;
using System.Collections.Generic;

public class Dummy : Creature
{
    public Dummy(string name, int maxHealth, Element element) : base(name, maxHealth, element)
    {
    }

    public override void Act()
    {
        battle.Logger.Log($"{Name} attacks you!");
        battle.Witch.Hurt(new Attack(2, Element.None, new List<string>()));
    }

    public override void Hurt(Attack attack)
    {
        int damage = attack.Power;
        Health = Math.Max(0, Health - damage);
        battle.Logger.Log($"{Name} took {damage} damage!");
    }
}
