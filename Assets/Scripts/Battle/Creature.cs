public abstract class Creature : Battler
{
    public Element Element { get; private set; }

    public Creature(string name, int maxHealth, Element element) : base(name, maxHealth)
    {
        Element = element;
    }

    protected int DamageTaken(Attack attack)
    {
        int advantage = Battle.CompareElements(attack.Element, Element);
        int damage = attack.Power;

        if (advantage > 0)
            damage *= 2;
        else if (advantage < 0)
            damage /= 2;

        return damage;
    }
}
