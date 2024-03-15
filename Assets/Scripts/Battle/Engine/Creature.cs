using System;

public abstract class Creature : Battler
{
    public Element Element { get; private set; }

    public Creature(string name, int maxHealth, Element element) : base(name, maxHealth)
    {
        Element = element;
    }

    public Tuple<int, int> GetDamageTaken(Attack attack)
    {
        if (battle.Cheats[(int)Cheats.InfiniteDamage])
        {
            return Tuple.Create(99999, 1);
        }

        int advantage = battle.CompareElements(attack.Element, Element);
        int damage = attack.Power;
        int reactionType = 0; //-1-Weak, 0-Normal, 1-Strong

        if (advantage > 0)
        {
            damage = (int)MathF.Floor(damage * battle.DamagePositiveMod);
            reactionType = 1;
        }
        else if (advantage < 0)
        {
            damage = (int)MathF.Floor(damage * battle.DamageNegativeMod);
            reactionType = -1;
        }

        return Tuple.Create(damage, reactionType);
    }

}
