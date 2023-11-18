using System.Collections.Generic;

public struct Card
{
    public static Card None => new Card(CardType.None, Element.None, 0);

    public CardType Type { get; }
    public int Power { get; }
    public Element Element { get; }

    public Card(CardType type, Element element, int power)
    {
        Type = type;
        Power = power;
        Element = element;
    }

    public override string ToString()
    {
        return $"{Element} {Type} {Power}";
    }
}
