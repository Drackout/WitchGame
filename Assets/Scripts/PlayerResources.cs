using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private int totalDecks = 3;

    private int[] stones;
    private int gold;
    private IList<Card>[] decks;

    public static PlayerResources Instance { get; private set; }

    public int Gold
    {
        get => gold;
        set
        {
            gold = Math.Max(0, value);
        }
    }

    public IList<Card> OwnedCards { get; private set; }
    public IList<Card>[] Decks => decks;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        int elementCount = Enum.GetNames(typeof(Element)).Length;
        stones = new int[elementCount];

        OwnedCards = new List<Card>
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

        decks = new IList<Card>[totalDecks];

        decks[0] = OwnedCards.Take(20).ToList();
    }

    public int GetStones(Element element)
    {
        return stones[(int)element];
    }

    public void SetStones(Element element, int value)
    {
        stones[(int)element] = Math.Max(0, value);
    }
}
