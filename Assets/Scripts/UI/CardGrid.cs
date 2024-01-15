using System;
using UnityEngine;

public class CardGrid : MonoBehaviour
{
    [SerializeField] private GameObject gridRoot;
    [SerializeField] private UICardCreation cardPrefab;

    private PlayerResources pr;

    public void UpdateCard(int index)
    {
        GameObject cardObj = gridRoot.transform.GetChild(index).gameObject;
        var uiCard = cardObj.GetComponent<UICardCreation>();
        uiCard.Create(pr.OwnedCards[index]);
    }

    private void Start()
    {
        pr = PlayerResources.Instance;

        for (int i = 0; i < pr.OwnedCards.Count; i++)
        {
            InstantiateCard(i);
        }
    }

    private void InstantiateCard(int index)
    {
        Card card = pr.OwnedCards[index];

        UICardCreation uiCard = Instantiate(cardPrefab, gridRoot.transform);
        uiCard.Create(card);

        var enchantHandler = uiCard.GetComponent<EnchantHandler>();
        enchantHandler.CardIndex = index;
        enchantHandler.OnStonePlaced += (Element el, int i) => OnStonePlaced?.Invoke(el, i);
    }

    public event Action<Element, int> OnStonePlaced;
}
