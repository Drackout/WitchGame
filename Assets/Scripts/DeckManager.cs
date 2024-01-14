using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private CardGrid cardGrid;
    [SerializeField] private GameObject deckListRoot;
    [SerializeField] private ElementalStone[] stones;
    [SerializeField] private GameObject stoneDragMarkerPrefab;
    [SerializeField] private DeckCard deckCardPrefab;
    [SerializeField] private Sprite fireIcon;
    [SerializeField] private Sprite grassIcon;
    [SerializeField] private Sprite waterIcon;

    private PlayerResources pr;
    private GameObject dragMarker;

    private void Start()
    {
        pr = PlayerResources.Instance;

        mainMenuButton.onClick.AddListener(LoadMainMenu);
        cardGrid.OnStonePlaced += EnchantCard;
        foreach (ElementalStone s in stones)
        {
            s.OnStonePickup += StonePickedUp;
            s.OnStoneDrop += StoneDropped;
        }

        InstantiateDeckList();
    }

    private void InstantiateDeckList()
    {
        IDictionary<Card, int> cards = new Dictionary<Card, int>();
        for (int i = 0; i < pr.Decks[0].Count; i++)
        {
            if (!cards.ContainsKey(pr.Decks[0][i]))
            {
                cards[pr.Decks[0][i]] = 0;
            }
            cards[pr.Decks[0][i]] += 1;
        }

        foreach (Card k in cards.Keys)
        {
            DeckCard deckCard = Instantiate(deckCardPrefab, deckListRoot.transform);
            deckCard.Card = k;
            deckCard.Amount = cards[k];
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void EnchantCard(Element element, int index)
    {
        if (pr.GetStones(element) <= 0)
        {
            return;
        }

        Card card = pr.OwnedCards[index];
        if (card.Element != Element.None)
        {
            return;
        }

        pr.OwnedCards[index] = new Card(card.Type, element, card.Power);

        pr.SetStones(element, pr.GetStones(element) - 1);

        cardGrid.UpdateCard(index);
        foreach (ElementalStone s in stones)
        {
            s.UpdateAmount();
        }
    }

    private void StonePickedUp(Element element)
    {
        if (pr.GetStones(element) <= 0)
        {
            return;
        }

        dragMarker = Instantiate(stoneDragMarkerPrefab, transform);
        Image image = dragMarker.GetComponent<Image>();
        image.sprite = element switch
        {
            Element.Fire => fireIcon,
            Element.Grass => grassIcon,
            Element.Water => waterIcon,
            _ => throw new ArgumentException("Unknown element!")
        };
    }

    private void StoneDropped()
    {
        if (dragMarker != null)
        {
            Destroy(dragMarker);
            dragMarker = null;
        }
    }
}
