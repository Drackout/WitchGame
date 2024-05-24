using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<DeckSerializable> decks;
    public List<CardSerializable> ownedCards;
    public int stage;
    public int[] stones;
    public int gold;

    public SaveData()
    {
        decks = new List<DeckSerializable>();

        List<Card> defaultDeck = new List<Card>
        {
            new Card(CardType.Shield, Element.Water, 2),
            new Card(CardType.Heal, Element.Fire, 2),
            new Card(CardType.Sword, Element.Water, 3),
            new Card(CardType.Heal, Element.Grass, 2),
            new Card(CardType.Sword, Element.Grass, 3),
            new Card(CardType.Shield, Element.Grass, 2),
            new Card(CardType.Shield, Element.Water, 2),
            new Card(CardType.Sword, Element.Water, 2),
            new Card(CardType.Heal, Element.Fire, 2),
            new Card(CardType.Shield, Element.Grass, 1),
            new Card(CardType.Sword, Element.Fire, 2),
            new Card(CardType.Spell, Element.Water, 3),
            new Card(CardType.Spell, Element.Grass, 3),
            new Card(CardType.Spell, Element.Water, 2),
            new Card(CardType.Spell, Element.Fire, 2),
            new Card(CardType.Spell, Element.Water, 3),
            new Card(CardType.Shield, Element.Fire, 2),
            new Card(CardType.Spell, Element.Fire, 3),
            new Card(CardType.Heal, Element.Water, 2),
            new Card(CardType.Sword, Element.Grass, 2),
            new Card(CardType.Spell, Element.Grass, 2),
            new Card(CardType.Heal, Element.Grass, 2),
            new Card(CardType.Shield, Element.Water, 2),
            new Card(CardType.Shield, Element.Fire, 2),
            new Card(CardType.Spell, Element.Grass, 3),
            new Card(CardType.Sword, Element.Fire, 3),
            new Card(CardType.Shield, Element.Grass, 2),
            new Card(CardType.Heal, Element.Grass, 2)
        };
        decks.Add(new DeckSerializable(defaultDeck));

        ownedCards = new List<CardSerializable>();

        stage = 0;

        int elementCount = Enum.GetNames(typeof(Element)).Length;
        stones = new int[elementCount];

        gold = 0;
    }
}
