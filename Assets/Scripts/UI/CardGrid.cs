using System;
using UnityEngine;
using UnityEngine.UI;

public class CardGrid : MonoBehaviour
{
    [SerializeField] private GameObject gridRoot;
    [SerializeField] private UICardCreation cardPrefab;

    private PlayerResources pr;
    private GameObject cardDragIndicator;

    public void UpdateCard(int index)
    {
        GameObject cardObj = gridRoot.transform.GetChild(index).gameObject;
        var uiCard = cardObj.GetComponent<UICardCreation>();
        uiCard.Create(pr.OwnedCards[index]);
    }

    public void BuildGrid()
    {
        foreach (Transform t in gridRoot.transform)
        {
            Destroy(t.gameObject);
        }

        pr = PlayerResources.Instance;

        for (int i = 0; i < pr.OwnedCards.Count; i++)
        {
            InstantiateCard(i);
        }
    }

    private void Start()
    {
        BuildGrid();
    }

    private void InstantiateCard(int index)
    {
        Card card = pr.OwnedCards[index];

        UICardCreation uiCard = Instantiate(cardPrefab, gridRoot.transform);
        uiCard.Create(card);

        var enchantDropArea = uiCard.GetComponentInChildren<EnchantDropArea>();
        enchantDropArea.CardIndex = index;
        enchantDropArea.OnStonePlaced += (Element el, int i) => OnStonePlaced?.Invoke(el, i);

        var ownedCard = uiCard.GetComponent<OwnedCard>();
        ownedCard.Index = index;
    }

    private void HandleDrag(int index)
    {
        UICardCreation uiCard = Instantiate(cardPrefab, transform);
        uiCard.Create(pr.OwnedCards[index]);

        cardDragIndicator = uiCard.gameObject;

        Image image = cardDragIndicator.GetComponent<Image>();
        image.raycastTarget = false;

        cardDragIndicator.AddComponent<FollowMouse>();
    }

    private void HandleDrop()
    {
        Destroy(cardDragIndicator);
        cardDragIndicator = null;
    }

    public event Action<Element, int> OnStonePlaced;
}
