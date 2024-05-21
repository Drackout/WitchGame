using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class DeckSerializable
{
    public List<CardSerializable> cards;

    public DeckSerializable(IEnumerable<CardSerializable> cards)
    {
        this.cards = new List<CardSerializable>(cards);
    }

    public DeckSerializable(IEnumerable<Card> cards)
    {
        this.cards = cards.Select((Card c) => CardSerializable.FromCard(c)).ToList();
    }
}
