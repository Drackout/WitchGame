using System;
using System.Diagnostics;

public abstract class Creature : Battler
{
    public Element Element { get; private set; }

    public Creature(string name, int maxHealth, Element element) : base(name, maxHealth)
    {
        Element = element;
    }

    protected Tuple<int, int> DamageTaken(Attack attack)
    {
        int advantage = Battle.CompareElements(attack.Element, Element);
        int damage = attack.Power;
        int reactionType = 0; //-1-Weak, 0-Normal, 1-Strong

        if (advantage > 0)
        {
            damage *= 2;
            reactionType = 1;
        }
        else if (advantage < 0)
        {
            damage /= 2;
            reactionType = -1;
        }

        return Tuple.Create(damage, reactionType);
    }
}
