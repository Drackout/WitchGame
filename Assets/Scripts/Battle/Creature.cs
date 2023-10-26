public abstract class Creature : Battler
{
    public Element Element { get; private set; }

    public Creature(string name, int maxHealth, Element element) : base(name, maxHealth)
    {
        Element = element;
    }
}
