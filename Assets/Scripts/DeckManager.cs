using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private CardGrid cardGrid;
    [SerializeField] private ElementalStone[] stones;
    [SerializeField] private GameObject stoneDragMarkerPrefab;
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
