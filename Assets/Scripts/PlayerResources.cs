using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private int totalDecks = 3;

    [Min(0)]
    [SerializeField] private int minCardsInDeck;

    [Min(1)]
    [SerializeField] private int maxCardsInDeck;

    [Min(0)]
    [SerializeField] private int maxCopies;

    private int[] stones;
    private int gold;
    private IList<Card>[] decks;
    private IList<Card> neutralCards;

    public static PlayerResources Instance { get; private set; }

    public int Gold
    {
        get => gold;
        set
        {
            gold = Math.Max(0, value);

            SaveData save = SaveManager.Instance.SaveData;
            save.gold = gold;
        }
    }

    public IList<Card> OwnedCards { get; private set; }
    public IList<Card>[] Decks => decks;
    public IList<Card> NeutralCards => neutralCards;
    public int[] Stones => stones;
    public int MinCardsInDeck => minCardsInDeck;
    public int MaxCardsInDeck => maxCardsInDeck;
    public int MaxCopies => maxCopies;

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
        
        gold = 25;
        stones = new int[elementCount];
        stones[(int)Element.Fire] = 3;
        stones[(int)Element.Grass] = 2;
        stones[(int)Element.Water] = 2;

        neutralCards = new List<Card>();

        OwnedCards = new List<Card>
        {
            new Card(CardType.Shield, Element.None, 2),
            new Card(CardType.Heal, Element.None, 2),
            new Card(CardType.Shield, Element.None, 2),
            new Card(CardType.Heal, Element.None, 2),
            new Card(CardType.Sword, Element.None, 3)
        };

        decks = new IList<Card>[totalDecks];
        for (int i = 0; i < totalDecks; i++)
        {
            decks[i] = new List<Card>();
        }
    }

    private void Start()
    {
        SaveData saveData = SaveManager.Instance.SaveData;

        int i = 0;
        foreach (DeckSerializable deck in saveData.decks)
        {
            if (deck != null)
            {
                decks[i++] = deck.cards.Select((CardSerializable c) => c.ToCard()).ToList();
            }
        }

        OwnedCards = saveData.ownedCards.Select((CardSerializable c) => c.ToCard()).ToList();

        stones = saveData.stones;
        gold = saveData.gold;
    }

    public bool Obtain(Item item)
    {
        Debug.Log($"Obtained {item}");

        if (item is CardItem cardItem)
        {
            Card card = new Card(cardItem.type, cardItem.element, cardItem.power);
            AddCardToOwned(card);
        }
        else if (item is ElementalStoneItem stoneItem)
        {
            SetStones(stoneItem.element, GetStones(stoneItem.element) + 1);
        }

        return true;
    }

    public DeckChangeResult AddCardToDeck(int deck, Card card)
    {
        if (Decks[deck].Count + 1 > maxCardsInDeck)
        {
            return DeckChangeResult.TooManyCards;
        }

        int cardsOfTypeInDeck = Decks[deck]
            .Where((Card c) => c.Equals(card))
            .Count();
        if (cardsOfTypeInDeck + 1 > maxCopies)
        {
            return DeckChangeResult.TooManyCopies;
        }

        Decks[deck].Add(card);
        OnDeckChange?.Invoke(deck);
        return DeckChangeResult.Success;
    }

    public DeckChangeResult RemoveCardFromDeck(int deck, int index)
    {
        if (Decks[deck].Count - 1 < minCardsInDeck)
        {
            return DeckChangeResult.NotEnoughCards;
        }

        Decks[deck].RemoveAt(index);
        OnDeckChange?.Invoke(deck);
        return DeckChangeResult.Success;
    }

    public void AddCardToOwned(Card card)
    {
        OwnedCards.Add(card);
        OnOwnedChange?.Invoke();

        SaveManager sm = SaveManager.Instance;
        sm.SaveData.ownedCards = OwnedCards.Select((Card c) => CardSerializable.FromCard(c)).ToList();
    }

    public void RemoveCardFromOwned(int index)
    {
        OwnedCards.RemoveAt(index);
        OnOwnedChange?.Invoke();

        SaveManager sm = SaveManager.Instance;
        sm.SaveData.ownedCards = OwnedCards.Select((Card c) => CardSerializable.FromCard(c)).ToList();
    }

    public int GetStones(Element element)
    {
        return stones[(int)element];
    }

    public void SetStones(Element element, int value)
    {
        stones[(int)element] = Math.Max(0, value);

        SaveData save = SaveManager.Instance.SaveData;
        save.stones[(int)element] = stones[(int)element];
    }

    public event Action<int> OnDeckChange;
    public event Action OnOwnedChange;
}
