using System.Collections.Generic;

public struct Attack
{
    public int Power { get; }
    public Element Element { get; }
    public IEnumerable<string> Tags { get; }

    public Attack(int power, Element element, IEnumerable<string> tags)
    {
        Power = power;
        Element = element;
        Tags = tags;
    }
}
