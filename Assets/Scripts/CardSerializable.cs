using System;
using UnityEngine;

[Serializable]
public struct CardSerializable
{
    [SerializeField] private CardType type;
    [SerializeField] private int power;
    [SerializeField] private Element element;

    public static CardSerializable FromCard(Card c)
    {
        CardSerializable card = new CardSerializable(c.Type, c.Element, c.Power);
        return card;
    }

    public Card ToCard()
    {
        Card c = new Card(type, element, power);
        return c;
    }

    private CardSerializable(CardType type, Element element, int power)
    {
        this.type = type;
        this.power = power;
        this.element = element;
    }
}
