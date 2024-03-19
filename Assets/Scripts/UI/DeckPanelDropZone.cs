using UnityEngine;
using UnityEngine.EventSystems;

public class DeckPanelDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var ownedCard = eventData.pointerDrag.GetComponent<OwnedCard>();
        if (ownedCard == null)
        {
            return;
        }

        PlayerResources pr = PlayerResources.Instance;

        Card card = pr.OwnedCards[ownedCard.Index];
        if (card.Element == Element.None)
        {
            return;
        }

        pr.AddCardToDeck(0, card);
        pr.RemoveCardFromOwned(ownedCard.Index);
    }
}
