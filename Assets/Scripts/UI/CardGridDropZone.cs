using UnityEngine;
using UnityEngine.EventSystems;

public class CardGridDropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        PlayerResources pr = PlayerResources.Instance;

        var deckCard = eventData.pointerDrag.GetComponent<DeckCard>();
        if (deckCard == null)
        {
            return;
        }

        pr.AddCardToOwned(deckCard.Card);
        int index = pr.Decks[0].IndexOf(deckCard.Card);
        pr.RemoveCardFromDeck(0, index);
    }
}
